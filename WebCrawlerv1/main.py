import json
from time import sleep

from startup import Startup

try:
    # Load configuration.
    app_config: object

    with open("WebCrawler/appconfig.json") as config:
        app_config = json.load(config)

    init = Startup(app_config)

    init.init_crawler()
except Exception as ex:
    print(ex)
finally:
    sleep(10)
