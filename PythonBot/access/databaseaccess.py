import pyodbc
from objects.jobadvert import JobAdvert


class DatabaseAccess:
    """
    Handles the access to the database.
    """
    db: pyodbc.Connection
    cursor: str

    def __init__(self):
        self.db = pyodbc

    def connect(self, auto_commit: bool = False):
        """
        Initializes a new connection to the database.
        @param auto_commit: Optional, default is False.
        """
        conn = self.db.connect('Driver={SQL Server Native Client 11.0};'
                                       f'Server=172.17.0.62\\SKPMSSQLSERVER,1433;'
                                       f'Database=JobAgentDB;'
                                       f'UID=job_agent;'
                                       f'PWD=4Ndet0wn;', autocommit=auto_commit)
        self.cursor = conn.cursor()

    def get_source_links(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [CompanyId], [Link] FROM [SourceLink]')

    def get_existing_jobadvert_source_links(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [SourceURL] FROM [JobAdvert]')

    def get_categories(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [Id], [Name] FROM [Category] WHERE [Name] IS NOT NULL')

    def get_category_specializations(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [Id], [Name] FROM [Specialization] WHERE [Name] IS NOT NULL')

    def save_dataset(self, dataset: JobAdvert):
        self.connect(True)
        sp_sql = """\
        EXEC [CreateJobAdvert] @title=?, @email=?, @phoneNumber=?, @jobDescription=?, @jobLocation=?, @regDate=?, 
        @deadlineDate=?, @sourceURL=?, @companyId=?, @categoryId=?, @specializationId=?
        """
        params = (dataset.title, dataset.email, dataset.phone_number, dataset.description,
                  dataset.location, dataset.registered_date, dataset.deadline_date,
                  dataset.source_url, dataset.company_id, dataset.category_id,
                  dataset.category_specialization_id)

        self.cursor.execute(sp_sql, params)
