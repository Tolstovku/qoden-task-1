FROM java

COPY ./wait-for-it.sh .
RUN apt-get update \
    && apt-get install -y wget \
    && apt-get install unzip \
    && rm -rf /var/lib/apt/lists/*
RUN wget https://github.com/liquibase/liquibase/releases/download/liquibase-parent-3.6.3/liquibase-3.6.3-bin.tar.gz
RUN tar xzvf liquibase-3.6.3-bin.tar.gz && rm -rf liquibase-3.2.0-bin.tar.gz
RUN wget https://jdbc.postgresql.org/download/postgresql-42.2.5.jar
