FROM liquibase

COPY ./WebApplication1/Liquibase/Migrations/* ./Migrations/
COPY ./WebApplication1/Liquibase/Seeds/* ./Seeds/
COPY ./WebApplication1/Liquibase/index.xml ./
COPY ./run-migrations.sh ./
RUN chmod 777 ./run-migrations.sh
ENTRYPOINT ./run-migrations.sh
