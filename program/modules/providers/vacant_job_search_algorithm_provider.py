import logging as log
import re
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

            job_pages = self.web_data.load_page_sources(job_pages)
            vacant_jobs = self.__extract_vacant_jobs_from_job_pages(job_pages=job_pages)
            self.__handle_vacant_job_data(vacant_jobs)

        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __find_vacant_job_links(self) -> []:
        useful_links = []
        for word in self.manager.get_search_words():
            try:
                regex = re.compile(word[0], flags=re.IGNORECASE)
                links = self.soup.find_all('a', text=regex)
                if len(links) != int(0):
                    for a in links:
                        if a['href'] in useful_links:
                            continue
                        else:
                            useful_links.append(a['href'])
            except re.error as err:
                log.error(f'Failed checking for [{word[0]}]; Message: {err}')
                continue
        return useful_links

    def __scrape_vacant_job_data(self, job_page: object) -> []:
        if job_page is None:
            raise ValueError('Parameter was type of None.')
        else:
            if isinstance(job_page, JobPage):
                vacant_jobs = []

                if job_page.url is None:
                    log.warning(f'Could not process {job_page.id}, no url associated.')
                    return vacant_jobs

                self.initialize_soup(job_page.html_page_source)
                log.info(f'Processing {self.__class__.__name__} for Job Page [{job_page.id}] for Company [{job_page.company_id}].')
                for link in self.__find_vacant_job_links():
                    validated_url_result = self.__validate_vacant_job_url(job_page.url, link)
                    if validated_url_result == '':
                        log.warning(f'{link} is unreachable or invalid.')
                        continue

                    # log.info(f'Gathered [{validated_url_result}] for Company [{job_page.company_id}].')
                    vacant_job = VacantJob(
                        vacant_job_id=0,
                        link=validated_url_result,
                        company_id=job_page.company_id,
                        html_page_source=''
                    )

                    vacant_jobs.append(vacant_job)

                log.info(f'Loaded [{len(vacant_jobs)}] Vacant Jobs for Company [{job_page.company_id}].')
                return vacant_jobs
            else:
                raise TypeError('Given type was not of type JobPage.')

    def __validate_vacant_job_url(self, page_url: str,  link: str) -> str:
        # Check if the data url is reachable.
        if self.web_data.url_ok(link):
            return link

        # Format the url, then split on / to combine the link to the domain url.
        split_char = '/'
        split_url = page_url.split(split_char)[2]
        formatted_url = self.web_data.format_url(split_url + link)
        if self.web_data.url_ok(formatted_url):
            return formatted_url

        return ''

    def __extract_vacant_jobs_from_job_pages(self, job_pages: []) -> []:
        if job_pages is None:
            raise Exception('Expected parameter Job Page list, but got None.')
        else:
            vacant_jobs = []
            for page in job_pages:
                links = self.__scrape_vacant_job_data(page)
                for link in links:
                    vacant_jobs.append(link)

            return vacant_jobs

    def __handle_vacant_job_data(self, vacant_jobs: []) -> None:
        if vacant_jobs is None:
            raise ValueError(f'Parameter {type(vacant_jobs)} was not provided.')

        if len(vacant_jobs) == int(0):
            raise ValueError(f'Length of {type(vacant_jobs)} is 0.')

        log.info(f'Handling <{len(vacant_jobs)}> Vacant Jobs.')
        for vacant_job in vacant_jobs:
            try:
                if self.__vacant_job_exists(vacant_job):
                    continue
                    # log.info(f'Updating data for [{vacant_job.link}]..')
                    # self.manager.update_vacant_job(vacant_job)
                else:
                    # log.info(f'Creating new data for [{vacant_job.link}]..')
                    self.manager.create_vacant_job(vacant_job)
            except ValueError as err:
                log.warning(err)
                continue

    def __vacant_job_exists(self, vacant_job: VacantJob) -> bool:
        if not isinstance(vacant_job, VacantJob):
            raise ValueError(f'Parameter given, is not type of {type(VacantJob)}.')

        existing_vacant_jobs = set(self.manager.get_vacant_jobs())
        if existing_vacant_jobs is None:
            raise ValueError('Existing Vacant Jobs is None.')
        else:
            if len(existing_vacant_jobs) == 0:
                return False

            if not any(x.link == vacant_job.link for x in existing_vacant_jobs):
                # if vacant_job.link not in existing_vacant_jobs:
                # log.info(f'No duplicate found data for [{vacant_job.link}].')
                return False

            # log.info(f'Found duplicate data at [{vacant_job.link}].')
            return True

