import logging as log
from time import sleep
from re import match
from selenium import webdriver
from selenium.common.exceptions import WebDriverException
from program.modules.objects.vacant_job import VacantJob


class WebDataProvider:
    __app_config: object
    __sleep_timer: float

    def __init__(self,
                 app_config: object):
        self.__app_config = app_config
        self.__sleep_timer = self.get_sleep_timer()

    def load_page_sources_by_data_list(self, data_list: []) -> [VacantJob]:
        # Is the data list to return.
        output = []

        with self.__get_driver_instance() as driver:
            driver.minimize_window()

            for data in data_list:
                try:
                    self.__load_web_page(driver, data.link)

                    if driver.page_source is not None:
                        data_obj = VacantJob(
                            vacant_job_id=data.id,
                            link=data.link,
                            company_id=data.company_id,
                            html_page_source=driver.page_source
                        )
                        output.append(data_obj)

                except WebDriverException as driverEx:
                    log.info(driverEx)
                    continue

            return output

    def load_page_source_by_page_url(self, page_url: str):
        # Is the data list to return.
        with self.__get_driver_instance() as driver:
            driver.minimize_window()

            self.__load_web_page(driver, page_url)

            if driver.page_source is not None:
                return driver.page_source

            return ""

    def url_ok(self, url: str):
        from requests import head, RequestException
        try:
            r = head(self.format_url(url))
            return r.status_code == 200
        except RequestException:
            return False

    def __load_web_page(self, driver: webdriver, url: str):
        try:
            formatted_url = self.format_url(url)
            log.info(f'Loading web page from {formatted_url}')
            driver.get(url)
            sleep(self.__sleep_timer)
        except WebDriverException:
            raise WebDriverException(f'Error, could not load data for {url}')

    def __get_driver_path(self):
        web_driver_obj = self.__app_config["WebDriver"]
        base_path = web_driver_obj["BasePath"]
        driver_name = web_driver_obj["DriverName"]

        return f"{base_path}{driver_name}"

    def __get_driver_instance(self) -> webdriver:
        driver_instance = self.__app_config["WebDriver"]["DriverInstance"]
        driver_path = self.__get_driver_path()

        if driver_instance == "Edge":
            return webdriver.Edge(executable_path=driver_path)
        elif driver_instance == "Firefox":
            return webdriver.Firefox(executable_path=driver_path)
        elif driver_instance == "Chrome":
            return webdriver.Chrome(executable_path=driver_path)
        elif driver_instance == "Opera":
            return webdriver.Opera(executable_path=driver_path)
        elif driver_instance == "Headless":
            options = webdriver.FirefoxOptions()
            options.add_argument("--headless")
            return webdriver.Firefox(executable_path=driver_path, options=options)
        else:
            raise NotImplementedError('Given driver instance was not supported. '
                                      'Only supports [Edge, Firefox, Chrome, Opera] and Headless')

    @staticmethod
    def format_url(url: str):
        """
        Formats an URL, if it doesn't contain http | ftp | https.
        @param url: The URL to format.
        @return: A formatted URL.
        """
        if not match('(?:http|ftp|https)://', url):
            return 'https://{}'.format(url)
        return url

    def get_sleep_timer(self) -> float:
        webdriver_config = self.__app_config["WebDriver"]

        if 'SleepTimer' in webdriver_config:
            return webdriver_config["SleepTimer"]

        return 1
