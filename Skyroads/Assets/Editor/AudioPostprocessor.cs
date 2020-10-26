using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class AudioPostprocessor : AssetPostprocessor
{
    private float minSize = 200;
    private float maxSize = 5120;
    
    private void OnPreprocessAudio()
    {
        var fileInfo = new FileInfo(assetPath);
        float fileSize = (float)fileInfo.Length/1024;
        
        AudioImporter audioImporter = (AudioImporter)assetImporter;

        audioImporter.loadInBackground = true;
        audioImporter.preloadAudioData = true;
        
        AudioImporterSampleSettings sampleSettings = audioImporter.defaultSampleSettings;
        
        if (fileSize <= minSize)
        {
            sampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
        }
        else if (fileSize > minSize && fileSize < maxSize)
        {
            sampleSettings.loadType = AudioClipLoadType.CompressedInMemory;
        }
        else
        {
            sampleSettings.loadType = AudioClipLoadType.Streaming;
        }

        audioImporter.defaultSampleSettings = sampleSettings;
    }
}
