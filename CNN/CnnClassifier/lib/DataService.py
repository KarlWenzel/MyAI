import yaml
import CnnDbService
import FileService

class DataService:
	def __init__(self):
		self.config = yaml.safe_load(open(r".\app.yml"))
		print("remoteRoot: " + self.config["remoteRoot"])
		print("localRoot: " + self.config["localRoot"])
		
	def createInstanceSet(self, numTraining, numValidation, numTesting, seed=42, directoryWhiteList=[], directoryBlackList=[], requiredFeatureTypes=[], usesCrossValidation=False, notes=""):
		pass
		
	def 
	
def main():
	print("Testing DataService object")
	dataService = DataService()
	print("Testing Complete")
	
if __name__ == "__main__":
   main()