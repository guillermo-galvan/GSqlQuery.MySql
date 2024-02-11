﻿using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.Runner;
using GSqlQuery.Runner.Transforms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Test.Transform
{
    public class AddressTransform(int numColumns) : TransformTo<Address, MySqlDataReader>(numColumns)
    {
        private static T GetValue<T>(PropertyOptionsInEntity column, MySqlDataReader reader)
        {
            if (!column.Ordinal.HasValue)
            {
                return (T)column.DefaultValue;
            }
            else
            {
                var type = typeof(T);

                if (type == typeof(MySqlGeometry))
                {
                    object result = reader.GetMySqlGeometry(column.Ordinal.Value);
                    return (T)result;
                }

                var value = reader.GetValue(column.Ordinal.Value);
                return (T)TransformTo.SwitchTypeValue(type, value);
            }
        }

        public override Address Generate(IEnumerable<PropertyOptionsInEntity> columns, MySqlDataReader reader)
        {
            PropertyOptionsInEntity column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.AddressId));
            long addressId = GetValue<long>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.Address1));
            string address1 = GetValue<string>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.Address2));
            string address2 = GetValue<string>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.District));
            string district = GetValue<string>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.CityId));
            long cityId = GetValue<long>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.PostalCode));
            string postalCode = GetValue<string>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.Phone));
            string phone = GetValue<string>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.Location));
            MySqlGeometry location = GetValue<MySqlGeometry>(column, reader);

            column = columns.First(x => x.Property.PropertyInfo.Name == nameof(Address.LastUpdate));
            DateTime lastUpdate = GetValue<DateTime>(column, reader);

            return new Address(addressId, address1, address2, district, cityId, postalCode, phone, location, lastUpdate);
        }

        public override Task<Address> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, MySqlDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}
