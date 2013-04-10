using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Framework.RenrenService
{
    public class RRServiceFlyweight<IdType, EntityType> where EntityType : PropertyChangedBase, INotifyPropertyChanged, new()
    {
        #region Data member
        private IDictionary<string, EntityType> _refTable = new Dictionary<string, EntityType>();
        #endregion

        #region Utinity function
        public static string GenerateKey<IdType>(IdType id, int page = -1)
        {
            return id.ToString() + "_" + page.ToString();
        }

        public static string GenerateDefaultKey()
        {
            return "DEFAULT_KEY";
        }
        #endregion

        #region Add
        public void Add(IdType id, EntityType model, int page = -1)
        {
            string key = id.ToString() + "_" + page.ToString();
            if (!_refTable.ContainsKey(key))
            {
                _refTable.Add(key, model);
            }
            else
            {
                _refTable[key] = model;
            }
        }

        public void Add(string key, EntityType model)
        {
            if (!_refTable.ContainsKey(key))
            {
                _refTable.Add(key, model);
            }
            else
            {
                _refTable[key] = model;
            }
        }
        #endregion

        #region Reset
        public void Reset()
        {
            _refTable.Clear();
        }

        public void ResetById(IdType id)
        {
            string key = id.ToString() + "_" + "-1";
            if (_refTable.ContainsKey(key))
            {
                _refTable.Remove(key);
            }
        }

        public void ResetByIdnPage(IdType id, int page)
        {
            string key = id.ToString() + "_" + page.ToString();
            if (_refTable.ContainsKey(key))
            {
                _refTable.Remove(key);
            }
        }

        public void ResetByKey(string key)
        {
            if (_refTable.ContainsKey(key))
            {
                _refTable.Remove(key);
            }
        }
        #endregion

        #region Get
        public EntityType Entity(IdType id, int page = -1)
        {
            string key = id.ToString() + "_" + page.ToString();
            if (_refTable.ContainsKey(key))
            {
                return _refTable[key];
            }
            else
            {
                return null;
            }
        }

        public EntityType Entity(string key)
        {
            if (_refTable.ContainsKey(key))
            {
                return _refTable[key];
            }
            else
            {
                return null;
            }
        }

        public EntityType this[IdType id, int page = -1]
        {
            get
            {
                return Entity(id, page);
            }
            set
            {
                Add(id, value, page);
            }
        }

        public EntityType this[string key]
        {
            get
            {
                return Entity(key);
            }
            set
            {
                Add(key, value);
            }
        }
        #endregion

        #region Does exist
        public bool Contains(IdType id, int page = -1)
        {
            string key = id.ToString() + "_" + page.ToString();
            return _refTable.ContainsKey(key);
        }

        public bool Contains(string key)
        {
            return _refTable.ContainsKey(key);
        }
        #endregion
    }
}
