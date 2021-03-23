import logging
from asyncio import sleep
from re import match

from WebCrawler.constants.environment_constant import DriverConstant

from selenium import webdriver
from selenium.common.exceptions import WebDriverException


class GeckoDriverProvider:
    def get_data(self, source_data: []) -> []:
        output = []
        driver = webdriver.Firefox(executable_path=f'{DriverConstant.get_geckodriver_path("DEVELOPMENT")}')
        driver.minimize_window()

        for data in source_data:
            try:
                url = self.__format_url(data[1])
                logging.info(f'Attempts to get data from -> {url}')

                driver.get(url)
                sleep(1)

                if driver.page_source is not None:
                    output.append(
                        [
                            str(driver.page_source),
                            str(url),
                            int(data[0])
                        ])
            except ValueError:
                driver.close()
                continue
            except WebDriverException:
                continue

        driver.quit()

        return output

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
