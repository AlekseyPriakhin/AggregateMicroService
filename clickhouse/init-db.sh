#!/bin/bash
set -e

#clickhouse-client --query "CREATE TABLE queue (id UUID, timestamp UInt64, courseId UUID, title String, status String, stagesCount UInt64) ENGINE = Kafka('broker:9092', 'course', 'consumer-group-2', 'JSONEachRow');"
clickhouse-client --query "CREATE TABLE queue (id UUID, timestamp UInt64, status String) ENGINE = Kafka('broker:9092', 'course', 'consumer-group-2', 'JSONEachRow');"

clickhouse-client --query "CREATE TABLE daily (day Date, total UInt64) ENGINE = SummingMergeTree() ORDER BY (day);"
clickhouse-client --query "CREATE MATERIALIZED VIEW consumer TO daily AS SELECT toDate(toDateTime(timestamp)) AS day, count() as total FROM default.queue GROUP BY day;"

clickhouse-client --query "CREATE TABLE daily_statuses (day Date, count UInt64, status String) ENGINE = SummingMergeTree() ORDER BY (status);"
clickhouse-client --query "CREATE MATERIALIZED VIEW consumer_statuses TO daily_statuses AS SELECT toDate(timestamp) AS day, count(*) as count, status FROM default.queue GROUP BY status, day;"

#clickhouse-client --query "CREATE TABLE statuses (day Date, status String) ENGINE = SummingMergeTree() ORDER BY (day);"
#clickhouse-client --query "CREATE MATERIALIZED VIEW consumer_st TO statuses AS SELECT toDate(timestamp) AS day, status AS status FROM default.queue;"
# title String, status String, stagesCount String,description Nullable(String)