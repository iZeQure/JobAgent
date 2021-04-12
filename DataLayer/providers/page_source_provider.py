import logging
import time
from re import match

from selenium import webdriver
from selenium.common.exceptions import WebDriverException


class PageSourceProvider:
    def get_html_page_source_data_list(self, vacant_jobs: []) -> []:
        """
        Gets a list of html page source.
        Args:
            vacant_jobs: A list of objects containing the link for the html page.
        Returns:
            output: A list of vacant jobs with the html page source.
        """
        # Is the data list to return.
        output = []

        try:
            with webdriver.Firefox(executable_path=f'{self.__get_geckodriver_path("DEVELOPMENT")}') as driver:
                driver.minimize_window()

                for vacant_job in vacant_jobs:
                    try:
                        url = self.__format_url(vacant_job[1])
                        logging.info(f'Attempts to get data from -> {url}')

                        driver.get(url)
                        time.sleep(1)

                        if driver.page_source is not None:
                            output.append([
                                vacant_job[0],
                                vacant_job[1],
                                vacant_job[2],
                                driver.page_source
                            ])
                    except Exception is WebDriverException:
                        continue
        except Exception is BaseException:
            logging.error('Something went wrong, while attempting to read from webdriver.')

        return output

    def get_links_from_company_page_url(self, company: []) -> []:
        pass

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

    @staticmethod
    def __get_geckodriver_path(environment: str) -> str:
        if environment == "PRODUCTION":
            return "C:\\Zombie_Crawler\\tools\\geckodriver.exe"
        if environment == "DEVELOPMENT":
            return "C:\\VirtualEnvironment\\geckodriver.exe"
