#!/bin/bash

# start zookeeper
#$KAFKA_HOME/bin/zookeeper-server-start.sh $KAFKA_HOME/config/zookeeper.properties &
/usr/local/kafka/bin/zookeeper-server-start.sh /usr/local/kafka/config/zookeeper.properties &

# start kafka broker
#$KAFKA_HOME/bin/kafka-server-start.sh $KAFKA_HOME/config/server.properties
/usr/local/kafka/bin/kafka-server-start.sh /usr/local/kafka/config/server.properties
