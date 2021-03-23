import logging

from WebCrawler.models.job_advert_model import JobAdvertModel
from WebCrawler.providers import GeckoDriverProvider, SearchAlgorithmProvider


class AlgorithmService:
    """
    Handles the communication with the available URLs for the raw data.
    """
    provider: SearchAlgorithmProvider
    gecko: GeckoDriverProvider
    raw_data = []

    def __init__(self, algorithm_provider: SearchAlgorithmProvider, gecko_provider: GeckoDriverProvider):
        self.provider = algorithm_provider
        self.gecko = gecko_provider

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

    def get_raw_data(self, source_links: []) -> []:
        """
        Achieves the raw data, from an URL.
        @param source_links: A list of URLs.
        @return: A dataset of raw html data.
        """
        return self.gecko.get_data(source_links)
