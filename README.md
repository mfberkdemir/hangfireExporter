# Hangfire Prometheus Exporter

This is an exporter that exposes information Hangfire.

## Usage

Parameters

dataProvider  Hangfire datastorage.

   mongo,mysql,postgres,memorystorage,sqlserver,redis,azureservicebusqueue,litedb

connectionString  Datastorage connection string.

dbName  Datastorage database name.


DataStorage MongoDB
Parameter (dataProvider,connectionString,dbName)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=mongo" -e "connectionString=mongodb://192.168.1.1:27017" -e "dbName=hangfire" --name myapp hangfireExporter
```

DataStorage SqlServer(SqlExpress and localDB include)
Parameter (dataProvider,connectionString)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=sqlserver" -e "connectionString=Server=(localdb)\MSSQLLocalDB; database=hangfire; integrated security=True;" --name myapp hangfireExporter
```

DataStorage Redis(StackExchange)
Parameter (dataProvider,connectionString)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=redis" -e "connectionString=192.168.1.1:6379" --name myapp hangfireExporter
```


DataStorage Azure Service Bus Queue
Parameter (dataProvider,connectionString)

Sample
```sh
docker run -d -p 5001:80 -e "dataProvider=azureservicebusqueue" -e "connectionString=..." --name myapp hangfireExporter
```

DataStorage LiteDB
Parameter (dataProvider,connectionString)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=litedb" -e "connectionString=filePath" --name myapp hangfireExporter
```

DataStorage Memory Storage
Parameter (dataProvider)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=memorystorage" --name myapp hangfireExporter
```


DataStorage Mysql
Parameter (dataProvider,connectionString)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=mysql" -e "connectionString=server=192.168.1.1;uid=root;pwd=admin;database=hangfire;Allow User Variables=True" --name myapp hangfireExporter
```


DataStorage Postgres
Parameter (dataProvider,connectionString)

Sample

```sh
docker run -d -p 5001:80 -e "dataProvider=postgres" -e "connectionString=User ID = postgres; Password = password; Host = 192.168.1.1; Port = 5432; Database = hangfire;" --name myapp hangfireExporter
```

