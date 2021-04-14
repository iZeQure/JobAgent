from program.modules.database.db import DatabaseMSSQL
from program.modules.objects.address import Address
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob


class DatabaseManager:
    __database: DatabaseMSSQL

    def __init__(self, database: DatabaseMSSQL):
        self.__database = database

    def get_crawler_information(self):
        output: []

        with self.__database.connect(database_scheme='ZombieCrawlerDB') as context:
            sp_sql = 'EXEC [GetInitializationInformation]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_algorithm_keywords_by_key_value(self, key_value: str):
        output: []

        with self.__database.connect(database_scheme='ZombieCrawlerDB') as context:
            sp_sql = 'EXEC [GetKeysByKeyValue] @key_value=?'
            context.execute(sp_sql, key_value)
            output = context.fetchall()

        return output

    def get_jobadvert_ids(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'SELECT [VacantJobId] FROM [JobAdvert];'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_categories(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'EXEC [JA.spGetCategories]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_specializations(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'EXEC [JA.spGetSpecializations]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_companies(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'EXEC [JA.spGetCompanies]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_vacant_jobs(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'EXEC [JA.spGetVacantJobs]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def get_existing_job_adverts(self):
        output: []

        with self.__database.connect() as context:
            sp_sql = 'SELECT [VacantJobId] FROM [JobAdvert]'
            context.execute(sp_sql)
            output = context.fetchall()

        return output

    def create_job_advert(self, job_advert: JobAdvert):
        with self.__database.connect(auto_commit=True) as context:
            sp_sql = """EXEC [JA.spCreateJobAdvert]
                    @vacantJobId=?, 
                    @categoryId=?, 
                    @specializationId=?,
                    @jobAdvertTitle=?, 
                    @jobAdvertSummary=?,
                    @jobAdvertDescription=?, 
                    @jobAdvertEmail=?, 
                    @jobAdvertPhoneNr=?,
                    @jobAdvertRegistrationDateTime=?, 
                    @jobAdvertApplicationDeadlineDateTime=?;
                    """
            params = (
                job_advert.id,
                job_advert.category_id,
                job_advert.specialization_id,
                job_advert.title,
                job_advert.summary,
                job_advert.description,
                job_advert.email,
                job_advert.phone_number,
                job_advert.registration_datetime,
                job_advert.application_deadline_datetime
            )
            context.execute(sp_sql, params)

    def create_address(self, address: Address):
        with self.__database.connect(auto_commit=True) as context:
            sp_sql = """EXEC [JA.spCreateAddress]
                    @jobAdvertVacantJobId=?,
                    @streetAddress=?,
                    @city=?,
                    @postalCode=?;
                    """
            params = (
                address.job_advert_vacant_job_id,
                address.street_address,
                address.city,
                address.country,
                address.postal_code
            )
            context.execute(sp_sql, params)

    def create_vacant_job(self, vacant_job: VacantJob):
        with self.__database.connect(auto_commit=True) as context:
            sp_sql = """EXEC [JA.spCreateVacantJob]
                    @vacantJobLink=?,
                    @companyId=?;    
                    """
            params = (
                vacant_job.link,
                vacant_job.company_id
            )
            context.execute(sp_sql, params)

    def update_job_advert(self, job_advert: JobAdvert):
        with self.__database.connect(auto_commit=True) as context:
            sp_sql = """EXEC [JA.spUpdateJobAdvert]
                    @vacantJobId=?, 
                    @categoryId=?, 
                    @specializationId=?,
                    @jobAdvertTitle=?, 
                    @jobAdvertSummary=?,
                    @jobAdvertDescription=?, 
                    @jobAdvertEmail=?, 
                    @jobAdvertPhoneNr=?,
                    @jobAdvertRegistrationDateTime=?, 
                    @jobAdvertApplicationDeadlineDateTime=?;
                    """
            params = (
                job_advert.id,
                job_advert.category_id,
                job_advert.specialization_id,
                job_advert.title,
                job_advert.summary,
                job_advert.description,
                job_advert.email,
                job_advert.phone_number,
                job_advert.registration_datetime,
                job_advert.application_deadline_datetime
            )
            context.execute(sp_sql, params)
