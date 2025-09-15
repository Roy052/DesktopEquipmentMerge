# DesktopEquipmentMerge
 <div>
    <h2> 게임 정보 </h2>
    <img src = "https://img.itch.zone/aW1nLzE5OTMyMDI0LnBuZw==/315x250%23c/LM7onR.png"><br>
    <img src="https://img.shields.io/badge/Unity-yellow?style=flat-square&logo=Unity&logoColor=FFFFFF"/>
    <h4> 개발 일자 : 2025.09 <br><br>
    게임 플레이 : 업로드 이전..
  </div>
  <div>
    <h2> 게임 설명 </h2>
    <h3> 스토리 </h3>
     가문에서 쫓겨나 무너져가는 마을에 도착한 당신.<br><br>
     이곳을 지키던 사람과 만나 장비를 합성하는 방법을 얻게 된다.<br><br>
     합성한 장비를 이용해 마을을 성장시켜야 한다.
    <h3> 게임 플레이 </h3>
    마우스 클릭.
     </div>
     
  <div>
    <h2> 게임 스크린샷 </h2>
      <table>
        <td><img src = "https://img.itch.zone/aW1hZ2UvMzMxNjQwNi8xOTkzMjAyNS5wbmc=/250x600/yPSEIc.png"></td>
      </table>
  </div>
  <div>
    <h2> 게임 플레이 영상 </h2>
    https://youtu.be/pC8ZTxhjmpc
  </div>
  <div>
  </div>

   <div>
       <h2> 주요 코드 </h2>
       <h4> Excel을 Json으로 변경 </h4>
    </div>
    <h4> 참조한 코드 </h4>
    <h4> https://github.com/Benzino/ExcelToJsonConverter/tree/master </h4>
```csharp
      public bool ConvertExcelFileToJson(string filePath, string outputPath)
      {
          Debug.Log("Excel To Json Converter: Processing: " + filePath);
          if (filePath.Contains("~$"))
              return false;
      
          DataSet excelData = GetExcelDataSet(filePath);
      
          if (excelData == null)
          {
              Debug.LogError("Excel To Json Converter: Failed to process file: " + filePath);
              return false;
          }
      
          string spreadSheetJson = "";
      
          // Process Each SpreadSheet in the excel file
          for (int i = 0; i < excelData.Tables.Count; i++)
          {
              spreadSheetJson = GetSpreadSheetJson(excelData, excelData.Tables[i].TableName);
              if (String.IsNullOrEmpty(spreadSheetJson))
              {
                  Debug.LogError("Excel To Json Converter: Failed to covert Spreadsheet '" + excelData.Tables[i].TableName + "' to json.");
                  return false;
              }
              else
              {
                  // The file name is the sheet name with spaces removed
                  string fileName = excelData.Tables[i].TableName.Replace(" ", string.Empty);
                  WriteTextToFile(spreadSheetJson, outputPath + "/" + fileName + ".dem");
                  Debug.Log("Excel To Json Converter: " + excelData.Tables[i].TableName + " successfully written to file.");
              }
          }
      
          return true;
      }

      private string GetSpreadSheetJson(DataSet excelDataSet, string sheetName)
      {
          // Get the specified table
          DataTable dataTable = excelDataSet.Tables[sheetName];
      
          // Remove empty columns
          for (int col = dataTable.Columns.Count - 1; col >= 0; col--)
          {
              bool removeColumn = true;
              foreach (DataRow row in dataTable.Rows)
              {
                  if (!row.IsNull(col))
                  {
                      removeColumn = false;
                      break;
                  }
              }
      
              if (removeColumn)
              {
                  dataTable.Columns.RemoveAt(col);
              }
          }
      
          // Remove columns which start with '~'
          Regex columnNameRegex = new Regex(@"^~.*$");
          for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
          {
              if (columnNameRegex.IsMatch(dataTable.Columns[i].ColumnName))
              {
                  dataTable.Columns.RemoveAt(i);
              }
          }
      
          // Serialze the data table to json string
          return Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
      }
```

<h4> Json 데이터 로드 </h4>
    </div>
    
```csharp
        void LoadFromJson<T>(SheetName sheet) where T : IRegistrable
        {
            string filePath = $"{pathHead}{sheet}{pathTail}";
        
            try
            {
                if (!File.Exists(filePath)) return;
        
                string jsonText = File.ReadAllText(filePath);
                var dataList = JsonUtility.FromJson<DataList<T>>($"{{\"data\":{jsonText}}}");
        
                foreach (var item in dataList.data)
                    item.Register();
            }
            catch (Exception e) when (
                e is FileNotFoundException ||
                e is DirectoryNotFoundException ||
                e is IOException)
            {
                Debug.Log($"[JSON Load Error] {e.Message}");
            }
        }
        
        void LoadFromJson<T, U>(SheetName sheet) where T : IRegistrable where U : IConvertable<T>
        {
            string filePath = $"{pathHead}{sheet}{pathTail}";
        
            try
            {
                if (!File.Exists(filePath)) return;
        
                string jsonText = File.ReadAllText(filePath);
                var dataList = JsonUtility.FromJson<DataList<U>>($"{{\"data\":{jsonText}}}");
        
                foreach (var item in dataList.data)
                {
                    T convertedItem = item.ConvertTo();
                    convertedItem.Register();
                }
            }
            catch (Exception e) when (
                e is FileNotFoundException ||
                e is DirectoryNotFoundException ||
                e is IOException)
            {
                Debug.Log($"[JSON Load Error] {e.Message}");
            }
        }
```
