import logging
from json import load

from startup import Startup


class Main:
    configuration: object

    def __init__(self):
        self.get_configuration()
        self.build_log_config()

    def get_configuration(self):
        # Build configuration file.
        with open('program/appconfig.json') as config:
            self.configuration = load(config)

    def build_log_config(self):
        # Build logging configuration.
        log_config = self.configuration['Logging']
        logging.basicConfig(
            level=log_config['Level']['INFO'],
            format=log_config['Format'],
            datefmt=log_config['DateFormat']
        )

    def start_application(self):
        startup = Startup(self.configuration)
        startup.build_service_collection()
        startup.initialize_zombie()


main = Main()
main.start_application()
