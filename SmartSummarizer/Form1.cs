using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartSummarizer
{
    public partial class Form1 : Form
    {
        private GeminiService _geminiService;
        private TextBox txtInput;
        private Button btnSummarize;
        private Button btnClear;
        private Button btnTestApi;
        private Button btnListModels;
        private RichTextBox rtbOutput;
        private Label lblStatus;
        private Label lblInputTitle;
        private Label lblOutputTitle;
        private GroupBox grpInput;
        private GroupBox grpOutput;
        private ProgressBar progressBar;

        public Form1()
        {
            InitializeComponent();
            _geminiService = new GeminiService();
            UpdateStatusBasedOnApiKey();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "SmartSummarizer - AI Text Summarizer";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
        }

        private void UpdateStatusBasedOnApiKey()
        {
            if (_geminiService.IsApiKeyConfigured())
            {
                lblStatus.Text = "✅ AI Summarizer Ready. Powered by Google Gemini.";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "⚠️ API Key Required. Add your Gemini API key in GeminiService.cs";
                lblStatus.ForeColor = Color.Orange;
            }
        }

        private async void btnTestApi_Click(object sender, EventArgs e)
        {
            btnTestApi.Enabled = false;
            btnTestApi.Text = "Testing...";
            lblStatus.Text = "🔍 Testing API connection...";

            try
            {
                string result = await _geminiService.TestApiKeyAsync();

                if (result.Contains("SUCCESS"))
                {
                    MessageBox.Show(result, "API Test - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "✅ API key is valid! Ready to summarize.";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    MessageBox.Show(result, "API Test - Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "❌ API test failed. Try clicking 'List Models' to see available models.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ Test failed. Check your connection.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnTestApi.Enabled = true;
                btnTestApi.Text = "Test API";
            }
        }

        private async void btnListModels_Click(object sender, EventArgs e)
        {
            btnListModels.Enabled = false;
            btnListModels.Text = "Loading...";
            lblStatus.Text = "🔍 Fetching available models from Google...";
            rtbOutput.Text = "Loading available models...\n\nPlease wait...";

            try
            {
                string result = await _geminiService.ListAvailableModels();

                // Format the output nicely
                rtbOutput.Text = "=== AVAILABLE GEMINI MODELS ===\n\n";
                rtbOutput.Text += result;

                // Also show in message box with the model names
                MessageBox.Show("Models list loaded!\n\nCheck the output area to see available models.\n\n" +
                    "Look for model names like: gemini-pro, gemini-1.0-pro, etc.",
                    "Models Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblStatus.Text = "✅ Models listed. Check the summary area for available models.";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                rtbOutput.Text = $"Error loading models:\n\n{ex.Message}";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ Failed to list models. Check your API key.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnListModels.Enabled = true;
                btnListModels.Text = "List Models";
            }
        }

        private async void btnSummarize_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Please enter some text to summarize.",
                    "Input Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtInput.Focus();
                return;
            }

            btnSummarize.Enabled = false;
            btnSummarize.Text = "Summarizing...";
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Marquee;
            rtbOutput.Text = "";
            rtbOutput.ForeColor = Color.Black;

            try
            {
                string summary;

                if (_geminiService.IsApiKeyConfigured())
                {
                    lblStatus.Text = "🤖 Calling Google Gemini API...";
                    summary = await _geminiService.GetSummaryAsync(inputText);
                }
                else
                {
                    lblStatus.Text = "📝 Using basic summarizer (add API key for AI)...";
                    await Task.Delay(500);
                    summary = GenerateBasicSummary(inputText);
                }

                if (!string.IsNullOrEmpty(summary) && !summary.StartsWith("Error:"))
                {
                    rtbOutput.Text = summary;
                    lblStatus.Text = "✅ Summary generated successfully!";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    rtbOutput.Text = summary ?? "Could not generate summary.";
                    rtbOutput.ForeColor = Color.Orange;
                    lblStatus.Text = "⚠️ Using basic summarizer (API issue)";
                    lblStatus.ForeColor = Color.Orange;

                    if (_geminiService.IsApiKeyConfigured())
                    {
                        string fallback = GenerateBasicSummary(inputText);
                        rtbOutput.Text = $"[API Failed - Using Basic Summary]\n\n{fallback}";
                    }
                }
            }
            catch (Exception ex)
            {
                rtbOutput.Text = $"Error: {ex.Message}\n\n{GenerateBasicSummary(inputText)}";
                rtbOutput.ForeColor = Color.Orange;
                lblStatus.Text = "❌ Error occurred. Using fallback.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSummarize.Enabled = true;
                btnSummarize.Text = "Summarize";
                progressBar.Visible = false;
            }
        }

        private string GenerateBasicSummary(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "No text provided.";

            string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (sentences.Length <= 3)
                return text.Trim();

            return $"{sentences[0].Trim()}. {sentences[1].Trim()}. {sentences[sentences.Length - 1].Trim()}.";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";
            rtbOutput.Text = "";
            txtInput.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                btnSummarize_Click(this, new EventArgs());
                return true;
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                btnClear_Click(this, new EventArgs());
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            int charCount = txtInput.Text.Length;
            if (charCount > 8000)
            {
                lblStatus.Text = "⚠️ Text is very long. May be truncated.";
                lblStatus.ForeColor = Color.Orange;
            }
            else if (charCount > 0 && _geminiService.IsApiKeyConfigured())
            {
                lblStatus.Text = "✅ Ready for Gemini AI summarization.";
                lblStatus.ForeColor = Color.Green;
            }
            else if (charCount > 0)
            {
                lblStatus.Text = "📝 Ready for basic summarization.";
                lblStatus.ForeColor = SystemColors.ControlText;
            }
            else
            {
                lblStatus.Text = "Enter text and click Summarize.";
                lblStatus.ForeColor = SystemColors.ControlText;
            }
        }
    }
}