﻿
namespace DbConnectionProvider.Abstractions
{
    public interface IDbConnectionStringProvider
    {
        public string ProvideFor(string databaseName);
        public int DataSourceCount();
    }
}
