from time import sleep
from re import match
from selenium import webdriver
from objects.jobadvert import JobAdvert
from providers.searchalgorithmprovider import SearchAlgorithmProvider


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
        driver.minimize_window()

        for link in source_links:
            try:
                formatted_url = self.format_url(link[1])

                driver.get(formatted_url)
                sleep(1)

                self.raw_data.append((str(driver.page_source), str(formatted_url), int(link[0])))
            except ValueError:
                driver.close()
                return None

        driver.quit()
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

    def find_jobadvert_match(self, raw_data: [str, str, int]) -> JobAdvert:
        """
        Algorithm to find jobadvert information, specified by the object.
        @param raw_data: Raw HTML data.
        @return: An JobAdvert object, containing the match of data.
        """

        self.provider.set_page_source(raw_data[0])

        if self.provider is not None:
            jobadvert = JobAdvert(
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
