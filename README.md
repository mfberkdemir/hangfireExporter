# Hangfire Prometheus Exporter

This exporter that exposes information Hangfire. [Türkçe açıklama.](https://mfberkdemir.github.io/posts/hangfire-exporter.html)

[Simple Grafana Dashboard](https://grafana.com/grafana/dashboards/10928)

![Api Response](https://github.com/mfberkdemir/hangfireExporter/blob/master/hangfireEkran.JPG "Api Response")


## Usage

```docker
docker pull mfberkdemir/hangfireexporter:latest
```
### Parameters                                                                             

|  Parameter Name  | Description                    | Values                                      |
| ---------------- |--------------------------------|---------------------------------------------|
| dataProvider     | Hangfire datastorage.          | mongo, mysql, postgres, memorystorage, redis, sqlserver, azureservicebusqueue, litedb       |
| connectionString | Datastorage connection string. |                                             |
| dbName           | Datastorage database name.     |                                             |


----
#### DataStorage MongoDB


Parameter (dataProvider,connectionString,dbName)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=mongo" -e "connectionString=mongodb://192.168.1.1:27017" -e "dbName=hangfire" --name myapp mfberkdemir/hangfireexporter:latest
```

---
#### DataStorage SqlServer(SqlExpress and localDB include)


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=sqlserver" -e "connectionString=Server=(localdb)\MSSQLLocalDB; database=hangfire; integrated security=True;" --name myapp mfberkdemir/hangfireexporter:latest
```
---
#### DataStorage Redis(StackExchange)


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=redis" -e "connectionString=192.168.1.1:6379" --name myapp mfberkdemir/hangfireexporter:latest
```

---
#### DataStorage Azure Service Bus Queue


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=azureservicebusqueue" -e "connectionString=..." --name myapp mfberkdemir/hangfireexporter:latest
```
---
#### DataStorage LiteDB


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=litedb" -e "connectionString=filePath" --name myapp mfberkdemir/hangfireexporter:latest
```
---
#### DataStorage Memory Storage


Parameter (dataProvider)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=memorystorage" --name myapp mfberkdemir/hangfireexporter:latest
```

---
#### DataStorage Mysql


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=mysql" -e "connectionString=server=192.168.1.1;uid=root;pwd=admin;database=hangfire;Allow User Variables=True" --name myapp mfberkdemir/hangfireexporter:latest
```

---
#### DataStorage Postgres


Parameter (dataProvider,connectionString)

Sample

```docker
docker run -d -p 5001:80 -e "dataProvider=postgres" -e "connectionString=User ID = postgres; Password = password; Host = 192.168.1.1; Port = 5432; Database = hangfire;" --name myapp mfberkdemir/hangfireexporter:latest
```
