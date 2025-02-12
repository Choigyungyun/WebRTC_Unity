internal class JanusDatas
{
    public static (string, string) PluginOption { get { return pluginOption; } set { pluginOption = value; } }
    public static string Session_ID { get { return session_id; } set { session_id = value; } }

    private static (string, string) pluginOption;
    private static string session_id = string.Empty;
}
