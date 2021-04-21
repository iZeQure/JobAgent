from program.modules.objects.base_job_entity import BaseJobEntity


class Company(BaseJobEntity):
    __job_page_url: str
    
    def __init__(self, company_id: int, job_page_url: str, html_page_source):
        super().__init__(company_id, html_page_source)
        
        self.__job_page_url = job_page_url
        
    @property
    def job_page_url(self):
        return self.__job_page_url
