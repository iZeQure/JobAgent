from program.modules.objects.base_job_entity import BaseJobEntity


class Company(BaseJobEntity):
    __cvr: int
    __name: str
    __contact_person: str
    __job_pages: []
    __vacant_jobs: []
    
    def __init__(self, company_id: int, cvr: int, name: str, contact_person: str, html_page_source):
        super().__init__(company_id, html_page_source)
        
        self.__cvr = cvr
        self.__name = name
        self.__contact_person = contact_person
        
    @property
    def cvr(self):
        return self.__cvr

    @property
    def name(self):
        return self.__name

    @property
    def contact_person(self):
        return self.__contact_person

    def set_job_pages(self, data: []):
        self.__job_pages = data

    def set_vacant_jobs(self, data: []):
        self.__vacant_jobs = data
