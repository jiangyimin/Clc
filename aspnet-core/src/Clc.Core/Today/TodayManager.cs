using System;
using Abp.Configuration;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Clc.Configuration;
using Clc.Extensions;
using Clc.Fields;
using Clc.Types;

namespace Clc.Today
{
    public class TodayManager : IDomainService
    {
        private readonly IAbpSession _abpSession;
        private readonly ISettingManager _settingManager;
        private readonly FieldProvider _fieldProvider;
        private readonly TypeProvider _typeProvider;
        public TodayManager(
            IAbpSession abpSession,
            ISettingManager settingManager,
            FieldProvider fieldProvider, 
            TypeProvider typeProvider)
        {
            _abpSession = abpSession;
            _settingManager = settingManager;
            _fieldProvider = fieldProvider;
            _typeProvider = typeProvider;
        }

        public string ToDayString
        {
            get 
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public int DepotId {
            get 
            {
                return _abpSession.GetDepotId();
            }
        }
    }
}