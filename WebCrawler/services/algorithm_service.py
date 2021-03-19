import logging
from re import match
from time import sleep

from selenium import webdriver
from selenium.common.exceptions import WebDriverException

from WebCrawler.models.job_advert_model import JobAdvertModel
from WebCrawler.providers.search_algorithm_provider import SearchAlgorithmProvider


class AlgorithmService:
    """
    Handles the communication with the available URLs for the raw data.
    """
    provider: SearchAlgorithmProvider
    raw_data = []

    def __init__(self, algorithm_provider: SearchAlgorithmProvider):
        self.provider = algorithm_provider

    def get_raw_data(self, source_links: []) -> []:
        """
        Achieves the raw data, from an URL.
        @param source_links: A list of URLs.
        @return: A dataset of raw html data.
        """
        driver = webdriver.Firefox(executable_path='C:\\VirtualEnvironment\\geckodriver.exe')
        # driver = webdriver.Firefox(executable_path='C:\\Zombie_Crawler\\tools\\geckodriver.exe')
        driver.minimize_window()

        for link in source_links:
            try:
                formatted_url = self.format_url(link[1])

                logging.info(f'Getting source data from: {formatted_url}')

                driver.get(formatted_url)
                sleep(1)

                self.raw_data.append((str(driver.page_source), str(formatted_url), int(link[0])))
            except ValueError:
                driver.close()
                continue
            except WebDriverException:
                logging.warning(f'Failed getting data from: {formatted_url}')
                continue

        driver.quit()

        logging.info('Source Data loaded successfully.')

        return self.raw_data

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

    def find_jobadvert_match(self, raw_data: [str, str, int]) -> JobAdvertModel:
        """
        Algorithm to find jobadvert information, specified by the object.
        @param raw_data: Raw HTML data.
        @return: An JobAdvert object, containing the match of data.
        """
        self.provider.set_page_source(raw_data[0])

        logging.info(f'Attempts to gather job advert data for: {raw_data[1]}')

        if self.provider is not None:
            jobadvert = JobAdvertModel(
                0,
                self.provider.find_title(),
                self.provider.find_email(),
                self.provider.find_phone_number(),
                self.provider.find_description(),
                self.provider.find_location(),
                self.provider.find_registration_date(),
                self.provider.find_deadline_date(),
                raw_data[1],
                raw_data[2],
                self.provider.find_category(),
                self.provider.find_specialization()
            )

            return jobadvert
        return None
