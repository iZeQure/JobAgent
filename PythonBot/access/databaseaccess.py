from objects.jobadvert import JobAdvert
import pyodbc


class DatabaseAccess:
    serverName = ""
    database = ""
    userId = ""
    userPwd = ""

    def __init__(self):
        self.serverName = "172.17.0.62\\SKPMSSQLSERVER,1433"
        self.database = "JobAgentDB"
        self.userId = "job_agent"
        self.userPwd = "4Ndet0wn"

    def get_source_links(self) -> []:
        conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                              f'Server={self.serverName};'
                              f'Database={self.database};'
                              f'UID={self.userId};'
                              f'PWD={self.userPwd};')

        output = conn.cursor().execute('SELECT [CompanyId], [Link] FROM [SourceLink]')
        return output

    def get_existing_jobadvert_source_links(self) -> []:
        conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                              f'Server={self.serverName};'
                              f'Database={self.database};'
                              f'UID={self.userId};'
                              f'PWD={self.userPwd};')

        output = conn.cursor().execute('SELECT [SourceURL] FROM [JobAdvert]')
        return output

    def save_dataset(self, dataset: JobAdvert):
        conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                              f'Server={self.serverName};'
                              f'Database={self.database};'
                              f'UID={self.userId};'
                              f'PWD={self.userPwd};')

        sql = f"EXEC [CreateJobAdvert] '{str(dataset.title)}', '{str(dataset.email)}', '{str(dataset.phone_number)}'," \
              f"'{str(dataset.description)}', '{str(dataset.location)}', '{str(dataset.reg_date)}', " \
              f"'{str(dataset.deadline_date)}', '{str(dataset.source_url)}'," \
              f"{str(dataset.company_id)}, {int(dataset.category_id)}, {int(dataset.category_specialization_id)}"
        conn.cursor().execute(sql)
        conn.cursor().commit()
