LibCSV

CSV�t�@�C���̓ǂݍ��݂��s��C#���C�u����

public class ZipCodeCSV
{
	public string ZipCode;
	public string Address;
}

var fileName = "c:\test.csv";
var source = new CSVSource<ZipCodeCSV>(fileName);
ZipCodeCSV data = source.ReadNext();
Console.WriteLine(String.Format("Zip {0}: {1}", data.ZipCode, data.Address));

