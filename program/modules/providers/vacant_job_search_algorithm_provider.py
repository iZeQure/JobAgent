import logging as log
from program.modules.managers.database_manager import DatabaseManager
from program.modules.objects.job_page import JobPage
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.objects.vacant_job import VacantJob
from program.modules.providers.web_data_provider import WebDataProvider


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, manager: DatabaseManager, web_data: WebDataProvider):
        super().__init__(manager, web_data)

    def run_algorithm(self):
        try:
            job_pages = self.manager.get_job_pages()

            if job_pages is None:
                raise ValueError('No Job Pages were found.')

            # Run through every job page.
            for page in job_pages:
                if page.url is None:
                    continue

                # Get the page source for every job page.
                page_source = self.web_data.load_page_source_by_page_url(page.url)
                page.set_page_source(page_source)

            vacant_jobs = self.__get_vacant_jobs(job_pages=job_pages)
            self.__save_vacant_jobs(vacant_jobs)

        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __find_vacant_job_links(self) -> []:
        import re
        patterns = ['opslag-container-container', 'dre-teaser-content']
        useful_links = []
        for pattern in patterns:
            regex = re.compile('.*' + pattern + '*.')
            for div in self.soup.find_all('div', {'class': regex}):
                for a in div.find_all('a'):
                    useful_links.append(a['href'])

        return useful_links

    def __get_data(self, job_page: object) -> []:
        if job_page is None:
            raise ValueError('Parameter was type of None.')
        else:
            if isinstance(job_page, JobPage):
                vacant_jobs = []

                if job_page.url is None:
                    log.warning(f'Could not process {job_page.id}, no url associated.')
                    return vacant_jobs

                log.info(f'Processing algorithm with [{job_page.id}] for [{job_page.url}].')
                self.set_page_source(job_page.html_page_source)

                for link in self.__find_vacant_job_links():
                    if self.web_data.url_ok(link):
                        log.info(f'Gathered [{link}] for {job_page.company_id}')
                        vacant_job = VacantJob(
                            vacant_job_id=0,
                            link=link,
                            company_id=job_page.company_id,
                            html_page_source='')
                        vacant_jobs.append(vacant_job)
                    else:
                        combined_link = self.web_data.format_url(f'{job_page.url.split("/")[2]}{link}')
                        if self.web_data.url_ok(combined_link):
                            log.info(f'Gathered [{combined_link}] for {job_page.company_id}')
                            vacant_job = VacantJob(
                                vacant_job_id=0,
                                link=combined_link,
                                company_id=job_page.company_id,
                                html_page_source='')
                            vacant_jobs.append(vacant_job)
                        else:
                            log.warning(f'{link} could not be gathered, is invalid.')

                log.info(f'Found [{len(vacant_jobs)}] for company [{job_page.company_id}]')
                return vacant_jobs
            else:
                raise TypeError('Given type was not of type JobPage.')

    def __get_vacant_jobs(self, job_pages: []) -> []:
        if job_pages is None:
            raise Exception('Expected parameter Job Page list, but got None.')
        else:
            vacant_jobs = []
            for job in job_pages:
                links = self.__get_data(job)
                for link in links:
                    vacant_jobs.append(link)

            return vacant_jobs

    def __save_vacant_jobs(self, vacant_jobs: []):
        log.info(f'Saving <{len(vacant_jobs)}> Vacant Jobs.')
        for job in vacant_jobs:
            self.manager.create_vacant_job(job)
