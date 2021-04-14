from json import load
from time import sleep

from startup import Startup, log

try:
    # Load configuration file.
    app_config: object

    with open('program/appconfig.json') as config:
        app_config = load(config)

    start = Startup(app_config)
    start.initialize_zombie()
except Exception as ex:
    log.exception(ex)
finally:
    sleep(10)
    quit(200)
