using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;

namespace ControlLibrary.Tools.Multimedia
{
    interface IAudioWriter
    {
        void WriteChunk(byte[] chunk, uint timeStamp);
        Task<bool> Finish();
        string FileName { get; }
    }

    #region AudioWriter

    class DummyAudioWriter : IAudioWriter
    {
        public DummyAudioWriter() { }

        public void WriteChunk(byte[] chunk, uint timeStamp) { }

        public async Task<bool> Finish() { return false; }

        public string FileName
        {
            get
            {
                return null;
            }
        }
    }

    class MP3Writer : IAudioWriter
    {
        string _name;
        MemoryStream _fs;
        List<string> _warnings;
        List<byte[]> _chunkBuffer;
        List<uint> _frameOffsets;
        uint _totalFrameLength;
        bool _isVBR;
        bool _delayWrite;
        bool _hasVBRHeader;
        bool _writeVBRHeader;
        int _firstBitRate;
        int _mpegVersion;
        int _sampleRate;
        int _channelMode;
        uint _firstFrameHeader;

        public MP3Writer(string name, List<string> warnings)
        {
            _name = name;
            _fs = new MemoryStream();
            _warnings = warnings;
            _chunkBuffer = new List<byte[]>();
            _frameOffsets = new List<uint>();
            _delayWrite = true;
        }

        public void WriteChunk(byte[] chunk, uint timeStamp)
        {
            _chunkBuffer.Add(chunk);
            ParseMP3Frames(chunk);
            if (_delayWrite && _totalFrameLength >= 65536)
            {
                _delayWrite = false;
            }
            if (!_delayWrite)
            {
                Flush();
            }
        }

        public async Task<bool> Finish()
        {
            var ok = false;
            Flush();
            if (_writeVBRHeader)
            {
                _fs.Seek(0, SeekOrigin.Begin);
                WriteVBRHeader(false);
            }
            //完成转换写入文件
            if (_fs != null && _fs.Length > 0)
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
                savePicker.FileTypeChoices.Add("音乐类型", new List<string>() { ".mp3" });

                savePicker.SuggestedFileName = _name;// +".mp3";
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    byte[] bytes = _fs.ToArray();
                    await FileIO.WriteBytesAsync(file, bytes);
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        ok = true;
                    }
                }
            }
            //_fs.Close();
            _fs.Dispose();
            return ok;
        }

        public string FileName
        {
            get
            {
                return _name;
            }
        }

        private void Flush()
        {
            foreach (byte[] chunk in _chunkBuffer)
            {
                _fs.Write(chunk, 0, chunk.Length);
            }
            _chunkBuffer.Clear();
        }

        private void ParseMP3Frames(byte[] buff)
        {
            int[] MPEG1BitRate = new int[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0 };
            int[] MPEG2XBitRate = new int[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 };
            int[] MPEG1SampleRate = new int[] { 44100, 48000, 32000, 0 };
            int[] MPEG20SampleRate = new int[] { 22050, 24000, 16000, 0 };
            int[] MPEG25SampleRate = new int[] { 11025, 12000, 8000, 0 };

            int offset = 0;
            int length = buff.Length;

            while (length >= 4)
            {
                ulong header;
                int mpegVersion, layer, bitRate, sampleRate, padding, channelMode;
                int frameLen;

                header = (ulong)BitConverterBE.ToUInt32(buff, offset) << 32;
                if (BitHelper.Read(ref header, 11) != 0x7FF)
                {
                    break;
                }
                mpegVersion = BitHelper.Read(ref header, 2);
                layer = BitHelper.Read(ref header, 2);
                BitHelper.Read(ref header, 1);
                bitRate = BitHelper.Read(ref header, 4);
                sampleRate = BitHelper.Read(ref header, 2);
                padding = BitHelper.Read(ref header, 1);
                BitHelper.Read(ref header, 1);
                channelMode = BitHelper.Read(ref header, 2);

                if ((mpegVersion == 1) || (layer != 1) || (bitRate == 0) || (bitRate == 15) || (sampleRate == 3))
                {
                    break;
                }

                bitRate = ((mpegVersion == 3) ? MPEG1BitRate[bitRate] : MPEG2XBitRate[bitRate]) * 1000;

                if (mpegVersion == 3)
                    sampleRate = MPEG1SampleRate[sampleRate];
                else if (mpegVersion == 2)
                    sampleRate = MPEG20SampleRate[sampleRate];
                else
                    sampleRate = MPEG25SampleRate[sampleRate];

                frameLen = GetFrameLength(mpegVersion, bitRate, sampleRate, padding);
                if (frameLen > length)
                {
                    break;
                }

                bool isVBRHeaderFrame = false;
                if (_frameOffsets.Count == 0)
                {
                    // Check for an existing VBR header just to be safe (I haven't seen any in FLVs)
                    int o = offset + GetFrameDataOffset(mpegVersion, channelMode);
                    if (BitConverterBE.ToUInt32(buff, o) == 0x58696E67)
                    { // "Xing"
                        isVBRHeaderFrame = true;
                        _delayWrite = false;
                        _hasVBRHeader = true;
                    }
                }

                if (isVBRHeaderFrame) { }
                else if (_firstBitRate == 0)
                {
                    _firstBitRate = bitRate;
                    _mpegVersion = mpegVersion;
                    _sampleRate = sampleRate;
                    _channelMode = channelMode;
                    _firstFrameHeader = BitConverterBE.ToUInt32(buff, offset);
                }
                else if (!_isVBR && (bitRate != _firstBitRate))
                {
                    _isVBR = true;
                    if (_hasVBRHeader) { }
                    else if (_delayWrite)
                    {
                        WriteVBRHeader(true);
                        _writeVBRHeader = true;
                        _delayWrite = false;
                    }
                    else
                    {
                        _warnings.Add("Detected VBR too late, cannot add VBR header.");
                    }
                }

                _frameOffsets.Add(_totalFrameLength + (uint)offset);

                offset += frameLen;
                length -= frameLen;
            }

            _totalFrameLength += (uint)buff.Length;
        }

        private void WriteVBRHeader(bool isPlaceholder)
        {
            byte[] buff = new byte[GetFrameLength(_mpegVersion, 64000, _sampleRate, 0)];
            if (!isPlaceholder)
            {
                uint header = _firstFrameHeader;
                int dataOffset = GetFrameDataOffset(_mpegVersion, _channelMode);
                header &= 0xFFFE0DFF; // Clear CRC, bitrate, and padding fields
                header |= (uint)((_mpegVersion == 3) ? 5 : 8) << 12; // 64 kbit/sec
                General.CopyBytes(buff, 0, BitConverterBE.GetBytes(header));
                General.CopyBytes(buff, dataOffset, BitConverterBE.GetBytes((uint)0x58696E67)); // "Xing"
                General.CopyBytes(buff, dataOffset + 4, BitConverterBE.GetBytes((uint)0x7)); // Flags
                General.CopyBytes(buff, dataOffset + 8, BitConverterBE.GetBytes((uint)_frameOffsets.Count)); // Frame count
                General.CopyBytes(buff, dataOffset + 12, BitConverterBE.GetBytes((uint)_totalFrameLength)); // File length
                for (int i = 0; i < 100; i++)
                {
                    int frameIndex = (int)((i / 100.0) * _frameOffsets.Count);
                    buff[dataOffset + 16 + i] = (byte)((_frameOffsets[frameIndex] / (double)_totalFrameLength) * 256.0);
                }
            }
            _fs.Write(buff, 0, buff.Length);
        }

        private int GetFrameLength(int mpegVersion, int bitRate, int sampleRate, int padding)
        {
            return ((mpegVersion == 3) ? 144 : 72) * bitRate / sampleRate + padding;
        }

        private int GetFrameDataOffset(int mpegVersion, int channelMode)
        {
            return 4 + ((mpegVersion == 3) ?
                ((channelMode == 3) ? 17 : 32) :
                ((channelMode == 3) ? 9 : 17));
        }
    }


    /*
    class MP3Writer : IAudioWriter
    {
        string _path;
        FileStream _fs;
        List<string> _warnings;
        List<byte[]> _chunkBuffer;
        List<uint> _frameOffsets;
        uint _totalFrameLength;
        bool _isVBR;
        bool _delayWrite;
        bool _hasVBRHeader;
        bool _writeVBRHeader;
        int _firstBitRate;
        int _mpegVersion;
        int _sampleRate;
        int _channelMode;
        uint _firstFrameHeader;

        public MP3Writer(string path, List<string> warnings)
        {
            _path = path;
            _fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 65536);
            _warnings = warnings;
            _chunkBuffer = new List<byte[]>();
            _frameOffsets = new List<uint>();
            _delayWrite = true;
        }

        public void WriteChunk(byte[] chunk, uint timeStamp)
        {
            _chunkBuffer.Add(chunk);
            ParseMP3Frames(chunk);
            if (_delayWrite && _totalFrameLength >= 65536)
            {
                _delayWrite = false;
            }
            if (!_delayWrite)
            {
                foreach (byte[] c in _chunkBuffer)
                {
                    _fs.Write(c, 0, c.Length);
                }
                _chunkBuffer.Clear();
            }
        }

        public void Finish()
        {
            if (_writeVBRHeader)
            {
                _fs.Seek(0, SeekOrigin.Begin);
                WriteVBRHeader(false);
            }
            _fs.Close();
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        private void ParseMP3Frames(byte[] buff)
        {
            int[] MPEG1BitRate = new int[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0 };
            int[] MPEG2XBitRate = new int[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 };
            int[] MPEG1SampleRate = new int[] { 44100, 48000, 32000, 0 };
            int[] MPEG20SampleRate = new int[] { 22050, 24000, 16000, 0 };
            int[] MPEG25SampleRate = new int[] { 11025, 12000, 8000, 0 };

            int offset = 0;
            int length = buff.Length;

            while (length >= 4)
            {
                ulong header;
                int mpegVersion, layer, bitRate, sampleRate, padding, channelMode;
                int frameLen;

                header = (ulong)BitConverterBE.ToUInt32(buff, offset) << 32;
                if (BitHelper.Read(ref header, 11) != 0x7FF)
                    break;

                mpegVersion = BitHelper.Read(ref header, 2);
                layer = BitHelper.Read(ref header, 2);
                BitHelper.Read(ref header, 1);
                bitRate = BitHelper.Read(ref header, 4);
                sampleRate = BitHelper.Read(ref header, 2);
                padding = BitHelper.Read(ref header, 1);
                BitHelper.Read(ref header, 1);
                channelMode = BitHelper.Read(ref header, 2);

                if ((mpegVersion == 1) || (layer != 1) || (bitRate == 0) || (bitRate == 15) || (sampleRate == 3))
                    break;

                bitRate = ((mpegVersion == 3) ? MPEG1BitRate[bitRate] : MPEG2XBitRate[bitRate]) * 1000;

                if (mpegVersion == 3)
                    sampleRate = MPEG1SampleRate[sampleRate];
                else if (mpegVersion == 2)
                    sampleRate = MPEG20SampleRate[sampleRate];
                else
                    sampleRate = MPEG25SampleRate[sampleRate];

                frameLen = GetFrameLength(mpegVersion, bitRate, sampleRate, padding);
                if (frameLen > length)
                    break;

                bool isVBRHeaderFrame = false;
                if (_frameOffsets.Count == 0)
                {
                    // Check for an existing VBR header just to be safe (I haven't seen any in FLVs)
                    int o = offset + GetFrameDataOffset(mpegVersion, channelMode);
                    if (BitConverterBE.ToUInt32(buff, o) == 0x58696E67)
                    { // "Xing"
                        isVBRHeaderFrame = true;
                        _delayWrite = false;
                        _hasVBRHeader = true;
                    }
                }

                if (isVBRHeaderFrame) { }
                else if (_firstBitRate == 0)
                {
                    _firstBitRate = bitRate;
                    _mpegVersion = mpegVersion;
                    _sampleRate = sampleRate;
                    _channelMode = channelMode;
                    _firstFrameHeader = BitConverterBE.ToUInt32(buff, offset);
                }
                else if (!_isVBR && (bitRate != _firstBitRate))
                {
                    _isVBR = true;
                    if (_hasVBRHeader) { }
                    else if (_delayWrite)
                    {
                        WriteVBRHeader(true);
                        _writeVBRHeader = true;
                        _delayWrite = false;
                    }
                    else
                        _warnings.Add("Detected VBR too late, cannot add VBR header.");
                }

                _frameOffsets.Add(_totalFrameLength + (uint)offset);

                offset += frameLen;
                length -= frameLen;
            }

            _totalFrameLength += (uint)buff.Length;
        }

        private void WriteVBRHeader(bool isPlaceholder)
        {
            byte[] buff = new byte[GetFrameLength(_mpegVersion, 64000, _sampleRate, 0)];
            if (!isPlaceholder)
            {
                uint header = _firstFrameHeader;
                int dataOffset = GetFrameDataOffset(_mpegVersion, _channelMode);
                header &= 0xFFFE0DFF; // Clear CRC, bitrate, and padding fields
                header |= (uint)((_mpegVersion == 3) ? 5 : 8) << 12; // 64 kbit/sec
                General.CopyBytes(buff, 0, BitConverterBE.GetBytes(header));
                General.CopyBytes(buff, dataOffset, BitConverterBE.GetBytes((uint)0x58696E67)); // "Xing"
                General.CopyBytes(buff, dataOffset + 4, BitConverterBE.GetBytes((uint)0x7)); // Flags
                General.CopyBytes(buff, dataOffset + 8, BitConverterBE.GetBytes((uint)_frameOffsets.Count)); // Frame count
                General.CopyBytes(buff, dataOffset + 12, BitConverterBE.GetBytes((uint)_totalFrameLength)); // File length
                for (int i = 0; i < 100; i++)
                {
                    int frameIndex = (int)((i / 100.0) * _frameOffsets.Count);
                    buff[dataOffset + 16 + i] = (byte)((_frameOffsets[frameIndex] / (double)_totalFrameLength) * 256.0);
                }
            }
            _fs.Write(buff, 0, buff.Length);
        }

        private int GetFrameLength(int mpegVersion, int bitRate, int sampleRate, int padding)
        {
            return ((mpegVersion == 3) ? 144 : 72) * bitRate / sampleRate + padding;
        }

        private int GetFrameDataOffset(int mpegVersion, int channelMode)
        {
            return 4 + ((mpegVersion == 3) ?
                ((channelMode == 3) ? 17 : 32) :
                ((channelMode == 3) ? 9 : 17));
        }
    }
    **/
    /**
    class SpeexWriter : IAudioWriter
    {
        const string _vendorString = "youtubeFisher";
        const uint _sampleRate = 16000;
        const uint _msPerFrame = 20;
        const uint _samplesPerFrame = _sampleRate / (1000 / _msPerFrame);
        const int _targetPageDataSize = 4096;

        string _path;
        FileStream _fs;
        int _serialNumber;
        List<OggPacket> _packetList;
        int _packetListDataSize;
        byte[] _pageBuff;
        int _pageBuffOffset;
        uint _pageSequenceNumber;
        ulong _granulePosition;

        public SpeexWriter(string path, int serialNumber)
        {
            _path = path;
            _serialNumber = serialNumber;
            _fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 65536);
            _fs.Seek((28 + 80) + (28 + 8 + _vendorString.Length), SeekOrigin.Begin); // Speex header + Vorbis comment
            _packetList = new List<OggPacket>();
            _packetListDataSize = 0;
            _pageBuff = new byte[27 + 255 + _targetPageDataSize + 254]; // Header + max segment table + target data size + extra segment
            _pageBuffOffset = 0;
            _pageSequenceNumber = 2; // First audio packet
            _granulePosition = 0;
        }

        public void WriteChunk(byte[] chunk, uint timeStamp)
        {
            int[] subModeSizes = new int[] { 0, 43, 119, 160, 220, 300, 364, 492, 79 };
            int[] wideBandSizes = new int[] { 0, 36, 112, 192, 352 };
            int[] inBandSignalSizes = new int[] { 1, 1, 4, 4, 4, 4, 4, 4, 8, 8, 16, 16, 32, 32, 64, 64 };
            int frameStart = -1;
            int frameEnd = 0;
            int offset = 0;
            int length = chunk.Length * 8;
            int x;

            while (length - offset >= 5)
            {
                x = BitHelper.Read(chunk, ref offset, 1);
                if (x != 0)
                {
                    // wideband frame
                    x = BitHelper.Read(chunk, ref offset, 3);
                    if (x < 1 || x > 4) goto Error;
                    offset += wideBandSizes[x] - 4;
                }
                else
                {
                    x = BitHelper.Read(chunk, ref offset, 4);
                    if (x >= 1 && x <= 8)
                    {
                        // narrowband frame
                        if (frameStart != -1)
                        {
                            WriteFramePacket(chunk, frameStart, frameEnd);
                        }
                        frameStart = frameEnd;
                        offset += subModeSizes[x] - 5;
                    }
                    else if (x == 15)
                    {
                        break; // terminator
                    }
                    else if (x == 14)
                    {
                        // in-band signal
                        if (length - offset < 4) goto Error;
                        x = BitHelper.Read(chunk, ref offset, 4);
                        offset += inBandSignalSizes[x];
                    }
                    else if (x == 13)
                    {
                        // custom in-band signal
                        if (length - offset < 5) goto Error;
                        x = BitHelper.Read(chunk, ref offset, 5);
                        offset += x * 8;
                    }
                    else goto Error;
                }
                frameEnd = offset;
            }
            if (offset > length) goto Error;

            if (frameStart != -1)
            {
                WriteFramePacket(chunk, frameStart, frameEnd);
            }

            return;

        Error:
            throw new Exception("Invalid Speex data.");
        }

        public void Finish()
        {
            WritePage();
            FlushPage(true);
            _fs.Seek(0, SeekOrigin.Begin);
            _pageSequenceNumber = 0;
            _granulePosition = 0;
            WriteSpeexHeaderPacket();
            WriteVorbisCommentPacket();
            FlushPage(false);
            _fs.Close();
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        private void WriteFramePacket(byte[] data, int startBit, int endBit)
        {
            int lengthBits = endBit - startBit;
            byte[] frame = BitHelper.CopyBlock(data, startBit, lengthBits);
            if (lengthBits % 8 != 0)
            {
                frame[frame.Length - 1] |= (byte)(0xFF >> ((lengthBits % 8) + 1)); // padding
            }
            AddPacket(frame, _samplesPerFrame, true);
        }

        private void WriteSpeexHeaderPacket()
        {
            byte[] data = new byte[80];
            General.CopyBytes(data, 0, Encoding.ASCII.GetBytes("Speex   ")); // speex_string
            General.CopyBytes(data, 8, Encoding.ASCII.GetBytes("unknown")); // speex_version
            data[28] = 1; // speex_version_id
            data[32] = 80; // header_size
            General.CopyBytes(data, 36, BitConverterLE.GetBytes((uint)_sampleRate)); // rate
            data[40] = 1; // mode (e.g. narrowband, wideband)
            data[44] = 4; // mode_bitstream_version
            data[48] = 1; // nb_channels
            General.CopyBytes(data, 52, BitConverterLE.GetBytes(unchecked((uint)-1))); // bitrate
            General.CopyBytes(data, 56, BitConverterLE.GetBytes((uint)_samplesPerFrame)); // frame_size
            data[60] = 0; // vbr
            data[64] = 1; // frames_per_packet
            AddPacket(data, 0, false);
        }

        private void WriteVorbisCommentPacket()
        {
            byte[] vendorStringBytes = Encoding.ASCII.GetBytes(_vendorString);
            byte[] data = new byte[8 + vendorStringBytes.Length];
            data[0] = (byte)vendorStringBytes.Length;
            General.CopyBytes(data, 4, vendorStringBytes);
            AddPacket(data, 0, false);
        }

        private void AddPacket(byte[] data, uint sampleLength, bool delayWrite)
        {
            OggPacket packet = new OggPacket();
            if (data.Length >= 255)
            {
                throw new Exception("Packet exceeds maximum size.");
            }
            _granulePosition += sampleLength;
            packet.Data = data;
            packet.GranulePosition = _granulePosition;
            _packetList.Add(packet);
            _packetListDataSize += data.Length;
            if (!delayWrite || (_packetListDataSize >= _targetPageDataSize) || (_packetList.Count == 255))
            {
                WritePage();
            }
        }

        private void WritePage()
        {
            if (_packetList.Count == 0) return;
            FlushPage(false);
            WriteToPage(BitConverterBE.GetBytes(0x4F676753U), 0, 4); // "OggS"
            WriteToPage((byte)0); // Stream structure version
            WriteToPage((byte)((_pageSequenceNumber == 0) ? 0x02 : 0)); // Page flags
            WriteToPage((ulong)_packetList[_packetList.Count - 1].GranulePosition); // Position in samples
            WriteToPage((uint)_serialNumber); // Stream serial number
            WriteToPage((uint)_pageSequenceNumber); // Page sequence number
            WriteToPage((uint)0); // Checksum
            WriteToPage((byte)_packetList.Count); // Page segment count
            foreach (OggPacket packet in _packetList)
            {
                WriteToPage((byte)packet.Data.Length);
            }
            foreach (OggPacket packet in _packetList)
            {
                WriteToPage(packet.Data, 0, packet.Data.Length);
            }
            _packetList.Clear();
            _packetListDataSize = 0;
            _pageSequenceNumber++;
        }

        private void FlushPage(bool isLastPage)
        {
            if (_pageBuffOffset == 0) return;
            if (isLastPage) _pageBuff[5] |= 0x04;
            uint crc = OggCRC.Calculate(_pageBuff, 0, _pageBuffOffset);
            General.CopyBytes(_pageBuff, 22, BitConverterLE.GetBytes(crc));
            _fs.Write(_pageBuff, 0, _pageBuffOffset);
            _pageBuffOffset = 0;
        }

        private void WriteToPage(byte[] data, int offset, int length)
        {
            Buffer.BlockCopy(data, offset, _pageBuff, _pageBuffOffset, length);
            _pageBuffOffset += length;
        }

        private void WriteToPage(byte data)
        {
            WriteToPage(new byte[] { data }, 0, 1);
        }

        private void WriteToPage(uint data)
        {
            WriteToPage(BitConverterLE.GetBytes(data), 0, 4);
        }

        private void WriteToPage(ulong data)
        {
            WriteToPage(BitConverterLE.GetBytes(data), 0, 8);
        }

        class OggPacket
        {
            public ulong GranulePosition;
            public byte[] Data;
        }
    }
    **/


    class AACWriter : IAudioWriter
    {
        string _name;
        MemoryStream _fs;
        int _aacProfile;
        int _sampleRateIndex;
        int _channelConfig;

        public AACWriter(string name)
        {
            this._name = name;
            _fs = new MemoryStream();
        }

        public void WriteChunk(byte[] chunk, uint timeStamp)
        {
            if (chunk.Length < 1) return;

            if (chunk[0] == 0)
            { // Header
                if (chunk.Length < 3) return;

                ulong bits = (ulong)BitConverterBE.ToUInt16(chunk, 1) << 48;

                _aacProfile = BitHelper.Read(ref bits, 5) - 1;
                _sampleRateIndex = BitHelper.Read(ref bits, 4);
                _channelConfig = BitHelper.Read(ref bits, 4);

                if ((_aacProfile < 0) || (_aacProfile > 3))
                    throw new Exception("Unsupported AAC profile.");
                if (_sampleRateIndex > 12)
                    throw new Exception("Invalid AAC sample rate index.");
                if (_channelConfig > 6)
                    throw new Exception("Invalid AAC channel configuration.");
            }
            else
            { // Audio data
                int dataSize = chunk.Length - 1;
                ulong bits = 0;

                // Reference: WriteADTSHeader from FAAC's bitstream.c

                BitHelper.Write(ref bits, 12, 0xFFF);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 2, 0);
                BitHelper.Write(ref bits, 1, 1);
                BitHelper.Write(ref bits, 2, _aacProfile);
                BitHelper.Write(ref bits, 4, _sampleRateIndex);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 3, _channelConfig);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 1, 0);
                BitHelper.Write(ref bits, 13, 7 + dataSize);
                BitHelper.Write(ref bits, 11, 0x7FF);
                BitHelper.Write(ref bits, 2, 0);

                _fs.Write(BitConverterBE.GetBytes(bits), 1, 7);
                _fs.Write(chunk, 1, dataSize);
            }
        }

        public async Task<bool> Finish()
        {
            var ok = false;
            //完成转换写入文件
            if (_fs != null && _fs.Length > 0)
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
                savePicker.FileTypeChoices.Add("音乐类型", new List<string>() { ".aac" });

                savePicker.SuggestedFileName = _name;// +".mp3";
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    byte[] bytes = _fs.ToArray();
                    await FileIO.WriteBytesAsync(file, bytes);
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        ok = true;
                    }
                }
            }
            //_fs.Close();
            _fs.Dispose();
            return ok;
        }

        public string FileName
        {
            get
            {
                return _name;
            }
        }
    }
    /*
    class WAVWriter : IAudioWriter
    {
        string _path;
        WAVToolsWriter _wr;
        int blockAlign;

        public WAVWriter(string path, int bitsPerSample, int channelCount, int sampleRate)
        {
            _path = path;
            _wr = new WAVToolsWriter(path, bitsPerSample, channelCount, sampleRate);
            blockAlign = (bitsPerSample / 8) * channelCount;
        }

        public void WriteChunk(byte[] chunk, uint timeStamp)
        {
            _wr.Write(chunk, chunk.Length / blockAlign);
        }

        public void Finish()
        {
            _wr.Close();
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        private class WAVToolsWriter
        {
            FileStream _fs;
            BinaryWriter _bw;
            int _bitsPerSample, _channelCount, _sampleRate, _blockAlign;
            long _sampleLen;

            public WAVToolsWriter(string path, int bitsPerSample, int channelCount, int sampleRate)
            {
                _bitsPerSample = bitsPerSample;
                _channelCount = channelCount;
                _sampleRate = sampleRate;
                _blockAlign = _channelCount * ((_bitsPerSample + 7) / 8);

                _fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                _bw = new BinaryWriter(_fs);

                WriteHeaders();
            }

            private void WriteHeaders()
            {
                const uint fccRIFF = 0x46464952;
                const uint fccWAVE = 0x45564157;
                const uint fccFormat = 0x20746D66;
                const uint fccData = 0x61746164;

                _bw.Write(fccRIFF);
                _bw.Write((uint)0);
                _bw.Write(fccWAVE);

                _bw.Write(fccFormat);
                _bw.Write((uint)16);
                _bw.Write((ushort)1);
                _bw.Write((ushort)_channelCount);
                _bw.Write((uint)_sampleRate);
                _bw.Write((uint)(_sampleRate * _blockAlign));
                _bw.Write((ushort)_blockAlign);
                _bw.Write((ushort)_bitsPerSample);

                _bw.Write(fccData);
                _bw.Write((uint)0);
            }

            public void Close()
            {
                const long maxFileSize = 0x7FFFFFFEL;
                long dataLen, dataLenPadded;

                dataLen = _sampleLen * _blockAlign;

                if ((dataLen & 1) == 1)
                {
                    _bw.Write((byte)0);
                }

                if ((dataLen + 44) > maxFileSize)
                {
                    dataLen = ((maxFileSize - 44) / _blockAlign) * _blockAlign;
                }

                dataLenPadded = ((dataLen & 1) == 1) ? (dataLen + 1) : dataLen;

                _bw.Seek(4, SeekOrigin.Begin);
                _bw.Write((uint)(dataLenPadded + 36));

                _bw.Seek(40, SeekOrigin.Begin);
                _bw.Write((uint)dataLen);

                _bw.Close();

                _bw = null;
                _fs = null;
            }

            public void Write(byte[] buff, int sampleCount)
            {
                if (sampleCount > 0)
                {
                    _fs.Write(buff, 0, sampleCount * _blockAlign);
                    _sampleLen += sampleCount;
                }
            }
        }

    }
    **/

    #endregion

    /* Utility */
    #region Utility classes

    public static class General
    {
        public static void CopyBytes(byte[] dst, int dstOffset, byte[] src)
        {
            System.Buffer.BlockCopy(src, 0, dst, dstOffset, src.Length);
        }
    }

    public static class BitHelper
    {
        public static int Read(ref ulong x, int length)
        {
            int r = (int)(x >> (64 - length));
            x <<= length;
            return r;
        }

        public static int Read(byte[] bytes, ref int offset, int length)
        {
            int startByte = offset / 8;
            int endByte = (offset + length - 1) / 8;
            int skipBits = offset % 8;
            ulong bits = 0;
            for (int i = 0; i <= Math.Min(endByte - startByte, 7); i++)
            {
                bits |= (ulong)bytes[startByte + i] << (56 - (i * 8));
            }
            if (skipBits != 0) Read(ref bits, skipBits);
            offset += length;
            return Read(ref bits, length);
        }

        public static void Write(ref ulong x, int length, int value)
        {
            ulong mask = 0xFFFFFFFFFFFFFFFF >> (64 - length);
            x = (x << length) | ((ulong)value & mask);
        }

        public static byte[] CopyBlock(byte[] bytes, int offset, int length)
        {
            int startByte = offset / 8;
            int endByte = (offset + length - 1) / 8;
            int shiftA = offset % 8;
            int shiftB = 8 - shiftA;
            byte[] dst = new byte[(length + 7) / 8];
            if (shiftA == 0)
            {
                System.Buffer.BlockCopy(bytes, startByte, dst, 0, dst.Length);
            }
            else
            {
                int i;
                for (i = 0; i < endByte - startByte; i++)
                {
                    dst[i] = (byte)((bytes[startByte + i] << shiftA) | (bytes[startByte + i + 1] >> shiftB));
                }
                if (i < dst.Length)
                {
                    dst[i] = (byte)(bytes[startByte + i] << shiftA);
                }
            }
            dst[dst.Length - 1] &= (byte)(0xFF << ((dst.Length * 8) - length));
            return dst;
        }
    }

    public static class BitConverterBE
    {
        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            return
                ((ulong)value[startIndex] << 56) |
                ((ulong)value[startIndex + 1] << 48) |
                ((ulong)value[startIndex + 2] << 40) |
                ((ulong)value[startIndex + 3] << 32) |
                ((ulong)value[startIndex + 4] << 24) |
                ((ulong)value[startIndex + 5] << 16) |
                ((ulong)value[startIndex + 6] << 8) |
                ((ulong)value[startIndex + 7]);
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            return
                ((uint)value[startIndex] << 24) |
                ((uint)value[startIndex + 1] << 16) |
                ((uint)value[startIndex + 2] << 8) |
                ((uint)value[startIndex + 3]);
        }

        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            return (ushort)(
                (value[startIndex] << 8) |
                (value[startIndex + 1]));
        }

        public static byte[] GetBytes(ulong value)
        {
            byte[] buff = new byte[8];
            buff[0] = (byte)(value >> 56);
            buff[1] = (byte)(value >> 48);
            buff[2] = (byte)(value >> 40);
            buff[3] = (byte)(value >> 32);
            buff[4] = (byte)(value >> 24);
            buff[5] = (byte)(value >> 16);
            buff[6] = (byte)(value >> 8);
            buff[7] = (byte)(value);
            return buff;
        }

        public static byte[] GetBytes(uint value)
        {
            byte[] buff = new byte[4];
            buff[0] = (byte)(value >> 24);
            buff[1] = (byte)(value >> 16);
            buff[2] = (byte)(value >> 8);
            buff[3] = (byte)(value);
            return buff;
        }

        public static byte[] GetBytes(ushort value)
        {
            byte[] buff = new byte[2];
            buff[0] = (byte)(value >> 8);
            buff[1] = (byte)(value);
            return buff;
        }
    }

    public static class BitConverterLE
    {
        public static byte[] GetBytes(ulong value)
        {
            byte[] buff = new byte[8];
            buff[0] = (byte)(value);
            buff[1] = (byte)(value >> 8);
            buff[2] = (byte)(value >> 16);
            buff[3] = (byte)(value >> 24);
            buff[4] = (byte)(value >> 32);
            buff[5] = (byte)(value >> 40);
            buff[6] = (byte)(value >> 48);
            buff[7] = (byte)(value >> 56);
            return buff;
        }

        public static byte[] GetBytes(uint value)
        {
            byte[] buff = new byte[4];
            buff[0] = (byte)(value);
            buff[1] = (byte)(value >> 8);
            buff[2] = (byte)(value >> 16);
            buff[3] = (byte)(value >> 24);
            return buff;
        }

        public static byte[] GetBytes(ushort value)
        {
            byte[] buff = new byte[2];
            buff[0] = (byte)(value);
            buff[1] = (byte)(value >> 8);
            return buff;
        }
    }

    public static class OggCRC
    {
        static uint[] _lut = new uint[256];

        static OggCRC()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint x = i << 24;
                for (uint j = 0; j < 8; j++)
                {
                    x = ((x & 0x80000000U) != 0) ? ((x << 1) ^ 0x04C11DB7) : (x << 1);
                }
                _lut[i] = x;
            }
        }

        public static uint Calculate(byte[] buff, int offset, int length)
        {
            uint crc = 0;
            for (int i = 0; i < length; i++)
            {
                crc = _lut[((crc >> 24) ^ buff[offset + i]) & 0xFF] ^ (crc << 8);
            }
            return crc;
        }
    }

    #endregion
}
