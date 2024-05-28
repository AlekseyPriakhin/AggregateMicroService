#!/bin/bash
set -e

clickhouse-client --query "CREATE TABLE queue (id UUID, timestamp UInt64, courseId UUID, title String, status String, stagesCount UInt64) ENGINE = Kafka('broker:9092', 'course', 'consumer-group-2', 'JSONEachRow');"
clickhouse-client --query "CREATE TABLE daily (day Date, courseId UUID, title String, status String) ENGINE = MergeTree() ORDER BY (day);"
clickhouse-client --query "CREATE MATERIALIZED VIEW consumer TO daily AS SELECT toDate(timestamp) AS day, courseId, title, status FROM default.queue"

# title String, status String, stagesCount String,description Nullable(String)