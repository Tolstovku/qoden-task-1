FROM liquibase

COPY Liquibase/Migrations/* ./Migrations/
COPY Liquibase/Seeds/* ./Seeds/
COPY Liquibase/index.xml ./
COPY ./run-migrations.sh ./
RUN chmod 777 ./run-migrations.sh
ENTRYPOINT ./run-migrations.sh
