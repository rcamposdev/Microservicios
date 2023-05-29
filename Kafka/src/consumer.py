import argparse
from kafka import KafkaConsumer

# python src/consumer.py -topic 'prueba-python'

# python src/consumer.py -topic 'transferencias-kafka'

parser = argparse.ArgumentParser()

parser.add_argument("-topic")

args = vars(parser.parse_args())

TOPIC_NAME =  args["topic"]

consumer = KafkaConsumer(TOPIC_NAME)

[print(message) for message in consumer]
    