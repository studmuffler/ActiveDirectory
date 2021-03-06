﻿using System.Collections.Generic;
using System.DirectoryServices;
using System;
using System.Linq;
using Landpy.ActiveDirectory.Attributes;
using Landpy.ActiveDirectory.Core;
using Landpy.ActiveDirectory.Core.Filter;
using Landpy.ActiveDirectory.Core.Filter.Expression;
using Landpy.ActiveDirectory.Entity.TypeAdapter;
using Landpy.ActiveDirectory.Entity.Attribute.Name;

namespace Landpy.ActiveDirectory.Entity.Object
{
    /// <summary>
    /// The computer AD object.
    /// </summary>
    public class ComputerObject : ADObject
    {
        private string objectSid;
        private string operatingSystemName;
        private string operatingSystemVersion;
        private string operatingSystemServicePack;
        private string dnsName;
        private string siteName;
        private IList<string> memberOf;
        private string managedBy;
        private ADObject managedByObject;

        /// <summary>
        /// The object sid.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.ObjectSid)]
        public string ObjectSid
        {
            get
            {
                if (String.IsNullOrEmpty(this.objectSid))
                {
                    this.objectSid = new SidAdapter(this.SearchResult.Properties[ComputerAttributeNames.ObjectSid]).Value;
                }
                return this.objectSid;
            }
        }

        /// <summary>
        /// The operating system name.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.OperatingSystem)]
        public string OperatingSystemName
        {
            get
            {
                if (String.IsNullOrEmpty(this.operatingSystemName))
                {
                    this.operatingSystemName = new SingleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.OperatingSystem]).Value;
                }
                return this.operatingSystemName;
            }
        }

        /// <summary>
        /// The operating system version.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.OperatingSystemVersion)]
        public string OperatingSystemVersion
        {
            get
            {
                if (String.IsNullOrEmpty(this.operatingSystemVersion))
                {
                    this.operatingSystemVersion = new SingleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.OperatingSystemVersion]).Value;
                }
                return this.operatingSystemVersion;
            }
        }

        /// <summary>
        /// The operating system service pack.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.OperatingSystemServicePack)]
        public string OperatingSystemServicePack
        {
            get
            {
                if (String.IsNullOrEmpty(this.operatingSystemServicePack))
                {
                    this.operatingSystemServicePack = new SingleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.OperatingSystemServicePack]).Value;
                }
                return this.operatingSystemServicePack;
            }
        }

        /// <summary>
        /// The dns name.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.DNSHostName)]
        public string DnsName
        {
            get
            {
                if (String.IsNullOrEmpty(this.dnsName))
                {
                    this.dnsName = new SingleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.DNSHostName]).Value;
                }
                return this.dnsName;
            }
        }

        /// <summary>
        /// The site.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.MsDS_SiteName)]
        public string SiteName
        {
            get
            {
                if (String.IsNullOrEmpty(this.siteName))
                {
                    this.siteName = new SingleLineAdapter(this.SearchResult, ComputerAttributeNames.MsDS_SiteName).Value;
                }
                return this.siteName;
            }
        }

        /// <summary>
        /// The member of groups.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.MemberOf)]
        public IList<string> MemberOf
        {
            get
            {
                if (this.memberOf == null)
                {
                    this.memberOf = new MultipleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.MemberOf]).Value;
                }
                return this.memberOf;
            }
            set
            {
                SetAttributeValue(ComputerAttributeNames.MemberOf, value);
                this.memberOf = value;
            }
        }

        /// <summary>
        /// The managed by user, group or contact distinguish name.
        /// </summary>
        [ADOriginalAttributeName(ComputerAttributeNames.ManagedBy)]
        public string ManagedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this.managedBy))
                {
                    this.managedBy = new SingleLineAdapter(this.SearchResult.Properties[ComputerAttributeNames.ManagedBy]).Value;
                }
                return this.managedBy;
            }
            set
            {
                this.DirectoryEntry.Properties[ComputerAttributeNames.ManagedBy].Value = value;
                this.managedBy = value;
            }
        }

        /// <summary>
        /// The managed by user, group or contact AD object.
        /// </summary>
        public ADObject ManagedByObject
        {
            get
            {
                if (this.managedByObject == null)
                {
                    this.managedByObject = FindOneByDN(this.ADOperator, this.ManagedBy);
                }
                return this.managedByObject;
            }
        }

        internal ComputerObject(IADOperator adOperator, SearchResult searchResult)
            : base(adOperator, searchResult)
        {
        }

        /// <summary>
        /// Fine one computer object by sid.
        /// </summary>
        /// <param name="adOperator">The AD operator.</param>
        /// <param name="sid">The sid.</param>
        /// <returns>One computer object.</returns>
        public static ComputerObject FindOneBySid(IADOperator adOperator, string sid)
        {
            return FindOneByFilter<ComputerObject>(adOperator, new Is(ComputerAttributeNames.ObjectSid, sid));
        }

        /// <summary>
        /// Fine one compter object by common name.
        /// </summary>
        /// <param name="adOperator">The AD operator.</param>
        /// <param name="cn">The common name.</param>
        /// <returns>One computer object.</returns>
        public static ComputerObject FindOneByCN(IADOperator adOperator, string cn)
        {
            return FindOneByFilter<ComputerObject>(adOperator, new And(new IsComputer(), new Is(AttributeNames.CN, cn)));
        }

        /// <summary>
        /// Find all computer objects.
        /// </summary>
        /// <param name="adOperator">The AD operator.</param>
        /// <returns>All computer objects.</returns>
        public static IList<ComputerObject> FindAll(IADOperator adOperator)
        {
            return FindAllByFilter<ComputerObject>(adOperator, new IsComputer());
        }

        /// <summary>
        /// Find all computer objects.
        /// </summary>
        /// <param name="adOperator">The AD operator.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>All computer objects by filter.</returns>
        public static IList<ComputerObject> FindAll(IADOperator adOperator, IFilter filter)
        {
            return FindAllByFilter<ComputerObject>(adOperator, new And(new IsComputer(), filter));
        }
    }
}