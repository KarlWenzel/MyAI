import pyodbc

class CnnDbService:
	SERVER = r"(localdb)\mssqllocaldb" 
	DATABASE = "CnnData"
	INTEGRATED_SECURITY = True
	USERNAME = "" 
	PASSWORD = "" 
	
	def __init__(self):
		self.setConnStr()
		
	def setConnStr(self):
		if CnnDbService.INTEGRATED_SECURITY:
			self.connStr = (
				r"Driver={SQL Server Native Client 11.0};"
				r"Server=" + CnnDbService.SERVER + ";"
				r"Database=" + CnnDbService.DATABASE + ";"
				r"Trusted_Connection=yes;"
			)
		else:
			self.connStr = (
				r"Driver={SQL Server Native Client 11.0};"
				r"Server=" + CnnDbService.SERVER + ";"
				r"Database=" + CnnDbService.DATABASE + ";"
				r"UID=" + CnnDbService.USERNAME + ";"
				r";PWD="+ CnnDbService.PASSWORD + ";"
			)
			
	def getLabels(self, labelCategory="PageSequence"):
		labels = []
		conn = pyodbc.connect(self.connStr)
		cursor = conn.cursor()
		try:
			sql = "SELECT LabelName FROM Labels WHERE CategoryName=?"
			cursor.execute(sql, labelCategory)
			row = cursor.fetchone()
			while row:
				labels.append(row[0])
				row = cursor.fetchone()
		except:
			print("Error while attempting to read from database")
		finally:
			conn.close()
		return labels
		
	def getLabelCategories(self):
		categories = []
		conn = pyodbc.connect(self.connStr)
		cursor = conn.cursor()
		try:
			sql = "SELECT CategoryName FROM LabelCategories"
			cursor.execute(sql) 
			row = cursor.fetchone() 
			while row: 
				categories.append(row[0])
				row = cursor.fetchone()
		except:
			print("Error while attempting to read from database")
		finally:
			conn.close()
		return categories
		
	def getDistinctFeatureValues(self, featureName, instanceSetID):
		featureValues = []
		conn = pyodbc.connect(self.connStr)
		cursor = conn.cursor()
		try:
			sql = (
				"SELECT DISTINCT Value FROM Instances inst "
				"INNER JOIN InstanceSetInstances instSet ON instSet.InstanceID=inst.ID "
				"INNER JOIN InstanceFeatures feat ON feat.InstanceID=inst.ID "
				"WHERE feat.FeatureName=? AND instSet.InstanceSetID=?"
			)
			cursor.execute(sql, featureName, instanceSetID)
			row = cursor.fetchone()
			while row:
				featureValues.append(row[0])
				row = cursor.fetchone()
		except:
			print("Error while attempting to read from database")
		finally:
			conn.close()
		return featureValues
		
	def createInstanceSet(self, numTrainig, numValidation, numTesting, seed=42, directoryWhiteList=[], directoryBlackList=[], requiredFeatureTypes=[], usesCrossValidation=False, notes=""):
		pass
	
	# def getImageFiles(self, trainingRole, category="PageSequence", take=None):
		# imageFiles = []
		# conn = pyodbc.connect(self.connStr)
		# cursor = conn.cursor()
		# try:
			# sql = "SELECT" if take is None else "SELECT TOP {}".format(take)
			# sql += (
				# "i.ImagePath, i.ImageInfoRole, CountyID, t.Text, i.ImageDirectoryName "
				# "FROM ImageInfoes i "
				# "INNER JOIN ImageDirectories d ON i.ImageDirectoryName=d.Text "
				# "INNER JOIN ImageTagImageInfoes ti ON ti.ImageInfo_ImagePath=i.ImagePath "
				# "INNER JOIN ImageTags t ON ti.ImageTag_Text=t.Text "
				# "WHERE t.Category=? "
				# "ORDER BY i.ImagePath "
			# )
			# cursor.execute(sql, category) 
			# row = cursor.fetchone() 
			# while row: 
				# imageFiles.append(ImageInfo(row[0], row[1], row[2], row[3], row[4]))
				# row = cursor.fetchone()
		# except:
			# print("Error while attempting to read from database")
		# finally:
			# conn.close()
		# return [x for x in imageFiles if x.image_role == training_role]
	
   
def main():
	print("Testing CnnDbService object")
	print()
	cnnDbService = CnnDbService()
	
	print("getLabelCategories()")
	for cat in cnnDbService.getLabelCategories():
		print(cat)
	print()
	
	print("getLabels()")
	for lab in cnnDbService.getLabels():
		print(lab)
	print()
	
	print("Testing Complete")
	
if __name__ == "__main__":
   main()