#!/bin/bash
echo "url = $DB_TYPE://$DB_HOST:$DB_PORT/$DB_NAME"
echo "user = $DB_USER"
./liquibase --changeLogFile=index.xml \
--username=$DB_USER \
--password=$DB_PASSWORD \
--url=$DB_TYPE://$DB_HOST:$DB_PORT/$DB_NAME \
--driver=org.postgresql.Driver \
--logLevel=$LOGLEVEL \
--classpath=./postgresql-42.2.5.jar \
--contexts=$CONTEXTS \
update
