import logging as log
from time import sleep
from re import match
from selenium import webdriver
from selenium.common.exceptions import WebDriverException
from selenium.webdriver import DesiredCapabilities

from program.modules.objects.job_page import JobPage
from program.modules.objects.vacant_job import VacantJob


class WebDataProvider:
    __app_config: object
    __timeout: float

    def __init__(self,
                 app_config: object):
        self.__app_config = app_config
        self.__timeout = self.get_timeout()

    def load_page_sources(self, pages: []) -> []:
        log.info(f'Loading [{len(pages)}] pages                                             ..')

        try:
            with self.__get_driver_instance() as driver:
                driver.minimize_window()
                for i, page in enumerate(pages):
                    if isinstance(page, JobPage):
                        try:
                            self.__load_web_page(driver, page.url)
                        except WebDriverException as ex:
                            log.error(ex)
                            continue
                    elif isinstance(page, VacantJob):
                        try:
                            self.__load_web_page(driver, page.link)
                        except WebDriverException as ex:
                            log.error(ex)
                            continue
                    else:
                        log.warning(f'Data type is undefined.')
                        continue

                    if driver.page_source is None:
                        page.set_page_source('')
                    else:
                        page.set_page_source(driver.page_source)

                    log.info(f'Done Loading [{i + 1}].')
        finally:
            log.info(f'Finished loading Job Pages.')

        return pages

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
            log.debug(f'Loading web page from {formatted_url}')
            driver.get(url)
            sleep(self.__timeout)
        except WebDriverException:
            raise WebDriverException(f'Error, could not load data for {url}')

    def __get_driver_path(self):
        web_driver_obj = self.__app_config["WebDriver"]
        base_path = web_driver_obj["BasePath"]
        driver_name = web_driver_obj["DriverName"]

        return f"{base_path}{driver_name}"

    def __get_driver_instance(self) -> webdriver:
        driver_config = self.__app_config["WebDriver"]
        driver_instance = driver_config["DriverInstance"]
        driver_path = self.__get_driver_path()

        if driver_instance == "Edge":
            edge = DesiredCapabilities.EDGE
            return webdriver.Edge(executable_path=driver_path, capabilities=edge)
        elif driver_instance == "Opera":
            opera = DesiredCapabilities.OPERA
            return webdriver.Opera(executable_path=driver_path, desired_capabilities=opera)
        elif driver_instance == "Firefox":
            firefox = DesiredCapabilities.FIREFOX
            if 'Options' in driver_config and 'Headless' in driver_config:
                ff_options = webdriver.FirefoxOptions()
                if driver_config["Headless"]:
                    ff_options.add_argument("--headless")
                [ff_options.add_argument(arg) for arg in driver_config["Options"]]
                return webdriver.Firefox(executable_path=driver_path, options=ff_options, desired_capabilities=firefox)
            return webdriver.Firefox(executable_path=driver_path, desired_capabilities=firefox)

        elif driver_instance == "Chrome":
            chrome = DesiredCapabilities.CHROME
            if 'Options' in driver_config and 'Headless' in driver_config:
                c_options = webdriver.ChromeOptions()
                if driver_config["Headless"]:
                    c_options.add_argument("--headless")
                [c_options.add_argument(arg) for arg in driver_config["Options"]]
                return webdriver.Chrome(executable_path=driver_path, options=c_options, desired_capabilities=chrome)
            return webdriver.Chrome(executable_path=driver_path, desired_capabilities=chrome)
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

    def get_timeout(self) -> float:
        webdriver_config = self.__app_config["WebDriver"]

        if 'Timeout' in webdriver_config:
            return webdriver_config["Timeout"]

        return 5
