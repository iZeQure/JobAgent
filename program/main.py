import logging
from json import load, JSONDecodeError
from time import sleep

from startup import Startup


class Main:
    __CONFIG_PATH = 'F:\\Configuration\\webcrawler\\appsettings.json'
    configuration: object

    def __init__(self):
        try:
            self.load_configuration()
            self.build_log_configuration()
        except ValueError as err:
            logging.error(err)

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
        except JSONDecodeError:
            logging.error(msg='Failed to decode configuration.')
            exit(0)

    def build_log_configuration(self):
        # Build logging configuration.
        logging_key = 'Logging'

        if logging_key in self.configuration:
            log_config = self.configuration[logging_key]
            logging.basicConfig(
                level=log_config['Level']['INFO'],
                format=log_config['Format'],
                datefmt=log_config['DateFormat']
            )
            return

        raise ValueError(f'Could not build logger, no such key <{logging_key}>.')

    def start_application(self):
        try:
            # Get an instance of startup, inject the configuration.
            startup = Startup(self.configuration)

            # Initialize needed services.
            startup.build_service_collection()

            # Start the Bot.
            startup.initialize_zombie()
        except ValueError as err:
            logging.error(err)


# Get an instance of main.
main = Main()

# Start main.
try:
    main.start_application()
except ValueError as valErr:
    logging.error(valErr)
except Exception as ex:
    logging.error(f'Something went wrong while starting application: {ex}')

# Final
logging.info('Destroying Crawler in 10 Seconds.')
sleep(10)
