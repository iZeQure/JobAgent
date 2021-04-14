import logging as log
from time import sleep
from re import match
from selenium import webdriver
from selenium.common.exceptions import WebDriverException


class WebDataProvider(object):
    __app_config: object

    def __init__(self,
                 app_config: object):
        self.__app_config = app_config

    def load_web_data_html(self, data_list: []):
        # Is the data list to return.
        output = []

        try:
            with webdriver.Firefox(executable_path=f'{self.__get_gecko_driver_path()}') as driver:
                driver.minimize_window()

                for data in data_list:
                    try:
                        url = self.__format_url(data[1])
                        log.info(f'Attempts to get data from -> {url}')

                        driver.get(url)
                        sleep(1)

                        if driver.page_source is not None:
                            output.append([
                                data[0],
                                data[1],
                                data[2],
                                driver.page_source
                            ])
                    except Exception is WebDriverException:
                        continue

                return output
        except Exception as ex:
            log.exception(ex)

    def load_vacant_jobs_from_company_job_page_url(self, company_list: []):
        pass

    def __get_gecko_driver_path(self):
        return self.__app_config["WebDriver"]["Path"]

    @staticmethod
    def __format_url(url: str):
        """
        Formats an URL, if it doesn't contain http | ftp | https.
        @param url: The URL to format.
        @return: A formatted URL.
        """
        if not match('(?:http|ftp|https)://', url):
            return 'https://{}'.format(url)
        return url
