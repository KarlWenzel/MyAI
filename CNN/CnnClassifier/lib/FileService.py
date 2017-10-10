import os
import shutil

class FileService:
	
	def __init__(self, rootDirectory=os.path.expanduser("~")):
		self.setRootDirectory(rootDirectory)
	
	def getRootDirectory(self):
		return self.RootDirectory
	
	def setRootDirectory(self, rootDirectory):
		self.RootDirectory = rootDirectory
	
	def copyDirectory(self, sourceDirectory, destinationDirectory, resizeDimensions=None, makeBlackAndWhite=False):
		if not os.path.exists(destinationDirectory):
			os.makedirs(destinationDirectory)
		fileList = os.listdir(sourceDirectory)
		for f in fileList:
			shutil.copy(os.path.join(sourceDirectory,f), destinationDirectory)
			
	def 
   
def main():
	print("Testing FileService object")
	print()
	fileService = FileService()
	
	print("Default Root Directory: " + fileService.getRootDirectory())
	
	fileService.copyDirectory(r"C:\Users\karl\Documents\TestA", r"C:\Users\karl\Documents\TestB")
	
	print("Testing Complete")
	
if __name__ == "__main__":
   main()
	