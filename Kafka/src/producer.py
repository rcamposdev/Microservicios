import argparse
from kafka import KafkaProducer

# python src/producer.py -message 'Soy un mensaje' -topic 'prueba-python'

parser = argparse.ArgumentParser()

parser.add_argument("-message")

parser.add_argument("-topic")

args = vars(parser.parse_args())

TOPIC_NAME = args["topic"]

MESSAGE = args["message"].encode('ascii')   

KAFKA_SERVER = "localhost:9092"

producer = KafkaProducer(bootstrap_servers=KAFKA_SERVER)

producer.send(TOPIC_NAME, MESSAGE)

producer.flush()

