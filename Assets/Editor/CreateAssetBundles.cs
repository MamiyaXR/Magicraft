using System.IO;
using UnityEditor;

public class CreateAssetBundles
{

	[MenuItem("AssetBundle/Package (Default)")]
	private static void PackageBuddle()
	{
		string packagePath = UnityEditor.EditorUtility.OpenFolderPanel("Select Package Path", "F:/", "");
		if (packagePath.Length <= 0 || !Directory.Exists(packagePath))
			return;
		BuildPipeline.BuildAssetBundles(packagePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
		AssetDatabase.Refresh();
	}
}
