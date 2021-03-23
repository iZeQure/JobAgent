class DriverConstant:
    @staticmethod
    def get_geckodriver_path(environment: str) -> str:
        if environment == "PRODUCTION":
            return "C:\\Zombie_Crawler\\tools\\geckodriver.exe"
        if environment == "DEVELOPMENT":
            return "C:\\VirtualEnvironment\\geckodriver.exe"

        return None
