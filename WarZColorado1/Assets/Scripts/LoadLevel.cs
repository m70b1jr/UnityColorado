using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1/4/20, as of now, this will be our scipt to read / load the level data.
public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int LoadLevel_Objects(float BarRange) //This is the LoadLevel_Objects function from WarZ (In c++) Currently need to convert to C# and take care of other things.
    {

        string path;
        path = @"C:\Users\m70b1\Documents\GitHub\UnityColorado\WarZColorado1\Assets\Scripts\LevelData.xml"; //manual file path at the moment 
        //= Path.GetTempFileName();
        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }

        /*sprintf(fname, "%s\\LevelData.xml", r3dGameLevel::GetHomeDir());
        r3dFile* f = r3d_open(fname, "rb");
        if (!f)
            return 0;*/

        char* fileBuffer = new char[f->size + 1];
        fread(fileBuffer, f->size, 1, f);
        fileBuffer[f->size] = 0;
        pugi::xml_document xmlLevelFile;
        pugi::xml_parse_result parseResult = xmlLevelFile.load_buffer_inplace(fileBuffer, f->size);
        fclose(f);
        if (!parseResult)
            r3dError("Failed to parse XML, error: %s", parseResult.description());
        pugi::xml_node xmlLevel = xmlLevelFile.child("level");

        g_leveldata_xml_ver->SetInt(0);
        if (!xmlLevel.attribute("version").empty())
        {
            g_leveldata_xml_ver->SetInt(xmlLevel.attribute("version").as_int());
        }


        if (g_level_settings_ver->GetInt() < 2 || g_level_settings_ver->GetInt() >= 3)
        {
            GameWorld().m_MinimapOrigin.x = xmlLevel.attribute("minimapOrigin.x").as_float();
            GameWorld().m_MinimapOrigin.z = xmlLevel.attribute("minimapOrigin.z").as_float();
            GameWorld().m_MinimapSize.x = xmlLevel.attribute("minimapSize.x").as_float();
            GameWorld().m_MinimapSize.z = xmlLevel.attribute("minimapSize.z").as_float();

            if (g_level_settings_ver->GetInt() < 2)
            {
                r_shadow_extrusion_limit->SetFloat(xmlLevel.attribute("shadowLimitHeight").as_float());
                if (!xmlLevel.attribute("near_plane").empty())
                {
                    r_near_plane->SetFloat(xmlLevel.attribute("near_plane").as_float());
                    r_far_plane->SetFloat(xmlLevel.attribute("far_plane").as_float());
                }
            }
        }

        if (GameWorld().m_MinimapSize.x == 0 || GameWorld().m_MinimapSize.z == 0)
        {
            GameWorld().m_MinimapSize.x = 100;
            GameWorld().m_MinimapSize.z = 100;
        }
        LoadLevelObjects(xmlLevel, BarRange);

        // delete only after we are done parsing xml!
       // delete[] fileBuffer;

        return 1;
    }
}
