import logging
from json import load, JSONDecodeError
from time import sleep

from startup import Startup


class Main:
    __CONFIG_PATH = 'C:\\Zombie_Crawler\\configuration\\appconfig.json'
    configuration: object

    def __init__(self):
        self.load_configuration()
        self.build_log_configuration()

    def load_configuration(self):
        # Build configuration file.
        try:
            with open(self.__CONFIG_PATH) as config:
                self.configuration = load(config)
        except FileNotFoundError:
            logging.error(msg='Could not get configuration: File not found.')
            exit(0)
        except FileExistsError:
            logging.error(msg='Could not get configuration: File already exists.')
            exit(0)
        except ValueError is JSONDecodeError:
            logging.error(msg='Failed to decode configuration.')
            exit(0)

    def build_log_configuration(self):
        # Build logging configuration.
        log_config = self.configuration['Logging']
        logging.basicConfig(
            level=log_config['Level']['INFO'],
            format=log_config['Format'],
            datefmt=log_config['DateFormat']
        )

    def start_application(self):
        # Get an instance of startup, inject the configuration.
        startup = Startup(self.configuration)

        # Initialize needed services.
        startup.build_service_collection()

        # Start the Bot.
        startup.initialize_zombie()


# Get an instance of main.
main = Main()

# Start main.
main.start_application()

# Final
logging.warning('Program closing in 10 seconds.')
sleep(10)
