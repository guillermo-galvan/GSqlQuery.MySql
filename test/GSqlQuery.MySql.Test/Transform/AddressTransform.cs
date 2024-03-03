using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.Runner;
using GSqlQuery.Runner.Transforms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;

namespace GSqlQuery.MySql.Test.Transform
{
    public class AddressTransform(int numColumns) : TransformTo<Address, MySqlDataReader>(numColumns)
    {
        public override Address CreateEntity(IEnumerable<PropertyValue> propertyValues)
        {
            long addressId = default;
            string address1 = default;
            string address2 = default;
            string district = default;
            long cityId = default;
            string postalCode = default;
            string phone = default;
            MySqlGeometry location = default;
            DateTime lastUpdate = default;

            foreach (PropertyValue item in propertyValues)
            {
                switch (item.Property.PropertyInfo.Name)
                {
                    case nameof(Address.AddressId):
                        addressId = (long)item.Value;
                        break;
                    case nameof(Address.Address1):
                        address1 = (string)item.Value;
                        break;
                    case nameof(Address.Address2):
                        address2 = (string)item.Value;
                        break;
                    case nameof(Address.District):
                        district = (string)item.Value;
                        break;
                    case nameof(Address.CityId):
                        cityId = (long)item.Value;
                        break;
                    case nameof(Address.PostalCode):
                        postalCode = (string)item.Value;
                        break;
                    case nameof(Address.Phone):
                        phone = (string)item.Value;
                        break;
                    case nameof(Address.Location):
                        location = (MySqlGeometry)item.Value;
                        break;
                    case nameof(Address.LastUpdate):
                        lastUpdate = (DateTime)item.Value;
                        break;
                    default:
                        break;
                }
            }

            return new Address(addressId, address1, address2, district, cityId, postalCode, phone, location, lastUpdate);
        }

        public override object GetValue(int ordinal, MySqlDataReader reader, Type propertyType)
        {
            if (propertyType == typeof(MySqlGeometry))
            {
                return  reader.GetMySqlGeometry(ordinal);
            }

            return base.GetValue(ordinal, reader, propertyType);
        }
    }
}
