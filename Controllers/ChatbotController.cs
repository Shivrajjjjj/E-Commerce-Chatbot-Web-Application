using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ECommerceChatbot.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatbotController : ControllerBase
    {
        private readonly ILogger<ChatbotController> _logger;

        private static readonly Dictionary<string, string> responses = new()
        {
            { "hello", "Hi! How can I assist you today?" },
            { "price", "Please specify the product name to check the price." },
            { "order status", "You can track your order in the 'My Orders' section." },
            { "bye", "Goodbye! Have a great day!" },
            { "return policy", "Our return policy lasts 30 days from the date of purchase." },
            { "payment options", "We accept credit cards, debit cards, UPI, and net banking." },
            { "delivery time", "Standard delivery takes 3-5 business days." },
            { "support", "You can contact support via chat or email at support@example.com." },
            { "discount", "Please check our Offers section for current discounts and deals." },
            { "account", "You can update your account details under the 'My Profile' section." },
            { "shipping", "Shipping is free for orders above $50." },
            { "cancel order", "You can cancel your order from the order details page." },
            { "track package", "Use the tracking ID provided in the confirmation email." },
            { "change address", "Go to 'My Profile' to update your shipping address." },
            { "wishlist", "You can save items to your wishlist for later purchase." },
            { "refund status", "Refunds are processed within 5-7 business days." },
            { "exchange policy", "We offer exchanges within 15 days of delivery." },
            { "invoice", "Invoices are available in the 'My Orders' section." },
            { "newsletter", "Subscribe to our newsletter to receive the latest updates." },
            { "loyalty program", "Earn points with every purchase and redeem them later." },
            { "gift cards", "Gift cards are available in the Gifts section." },
            { "offers", "Check our homepage or Offers section for current deals." },
            { "COD", "Cash on Delivery is available in select regions." },
            { "international shipping", "Yes, we ship internationally. Charges may apply." },
            { "feedback", "We value your feedback! Please share it with us." },
            { "app", "Download our app from the Play Store or App Store." },
            { "language", "You can change language preferences in your account settings." },
            { "size guide", "Refer to the size chart on each product page." },
            { "store location", "We are an online store, but you can find partner stores under Locations." },
            { "product warranty", "Product warranty details are available on product pages." },
            { "out of stock", "You can sign up to get notified when items are back in stock." },
            { "replacement", "Replacement is available for defective items within 10 days." },
            { "customer care", "Reach us via chat, email, or call 1800-123-456." },
            { "bulk order", "For bulk orders, please contact our sales team." },
            { "custom order", "We do not support custom orders currently." },
            { "subscription", "Manage your subscriptions under the 'Subscriptions' tab." },
            { "delivery charges", "Delivery is free above $50, else a $5 fee applies." },
            { "login issue", "Try resetting your password or contact support." },
            { "signup", "Create a new account with your email and password." },
            { "password reset", "Use the 'Forgot Password' option on the login page." },
            { "email update", "Update your email from the 'Account Settings' section." },
            { "phone number", "Change your phone number in profile settings." },
            { "order confirmation", "You will receive confirmation via email and SMS." },
            { "gift wrap", "Gift wrap is available at checkout for an additional fee." },
            { "returns pickup", "Schedule pickup from the Returns section." },
            { "store credit", "Refunds may also be issued as store credit." },
            { "preorder", "Preorder items ship once they’re available." },
            { "backorder", "Backordered items will be shipped as soon as restocked." },
            { "delivery partners", "We work with multiple national courier services." },
            { "business hours", "We operate 9 AM - 9 PM IST all days." },
            { "holiday delivery", "Delivery during holidays may be delayed slightly." },
            { "product authenticity", "All our products are 100% genuine and original." },
            { "EMI options", "Easy EMI options available on orders above $100." },
            { "chatbot", "Hi! I’m your shopping assistant. Ask me anything!" },
            { "track order", "Track your order using your Order ID here." },
            { "profile", "Access your profile from the top-right icon." },
            { "contact us", "Find all support options in the 'Contact Us' section." },
            { "FAQ", "Visit our FAQ page for quick help." },
            { "login", "Login to continue shopping and tracking your orders." },
            { "register", "Register now to unlock member benefits!" },
            { "save card", "Your card info can be saved securely during checkout." },
            { "remove item", "Go to cart and click the remove icon beside item." },
            { "cart", "Click on the cart icon to view or edit your cart." },
            { "language support", "Currently available in English and Hindi." },
            { "data privacy", "We ensure complete privacy of your personal data." },
            { "security", "All transactions are encrypted and secure." },
            { "browser issue", "Try refreshing or using a different browser." },
            { "loading issue", "Clear cache or check your internet connection." },
            { "product availability", "Product availability may vary by region." },
            { "price match", "We do not offer price matching currently." },
            { "eco friendly", "We use sustainable packaging materials." },
            { "seller info", "Seller details are available on the product page." },
            { "product care", "Refer to care instructions on the label or page." },
            { "category", "Explore categories from the main menu." },
            { "recommendation", "Based on your preferences, here are suggestions!" },
            { "download invoice", "Download from 'My Orders' > View Invoice." },
            { "reorder", "Click 'Reorder' in the order history." },
            { "notifications", "Enable push notifications for order updates." },
            { "app update", "Ensure your app is updated for latest features." },
            { "install app", "Install our app for the best shopping experience." },
            { "mobile version", "Our website is fully responsive for mobile." },
            { "browser support", "We recommend using Chrome or Edge." },
            { "device compatibility", "Compatible with all modern devices." },
            { "voice search", "Use the mic icon to search with voice." },
            { "ratings", "View customer ratings on product pages." },
            { "reviews", "Read verified buyer reviews before purchasing." },
            { "new arrivals", "Check out the latest arrivals section!" },
            { "best sellers", "Popular products are under the Best Sellers tab." },
            { "order delay", "Sorry! Some orders may be delayed due to demand." },
            { "covid safety", "We follow all health and hygiene protocols." },
            { "stock alert", "Get notified when item is back in stock." },
            { "notify me", "Click 'Notify Me' on the product page for updates." },
            { "billing info", "Update billing info from account settings." },
            { "tax invoice", "Yes, we provide GST tax invoices." },
            { "return reason", "Choose a reason when initiating return." },
            { "cancelled order", "Refund for cancelled order will reflect soon." },
            { "recommend a friend", "Share referral code to invite friends!" },
            { "account deactivation", "Contact support to deactivate your account." },
            { "download app", "Get the app from Play Store or App Store." },
            { "store credit balance", "Check it under 'My Wallet'." },
            { "apply coupon", "Enter coupon code at checkout to apply." },
            { "remove coupon", "Uncheck the coupon at checkout to remove it." }
        };

        public ChatbotController(ILogger<ChatbotController> logger)
        {
            _logger = logger;
        }

        [HttpGet("hello")]
        public IActionResult SayHello()
        {
            _logger.LogInformation("Hello endpoint was accessed.");
            return Ok(new { message = "Hello! How can I assist you with your shopping today?" });
        }

        [HttpPost("ask")]
        public IActionResult AskChatbot([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                _logger.LogWarning("Invalid question received.");
                return BadRequest(new { message = "Please ask a valid question." });
            }

            _logger.LogInformation($"Received question: {request.Question}");

            string response = GetChatbotResponse(request.Question);

            return Ok(new { reply = response });
        }

        private static string GetChatbotResponse(string question)
        {
            question = question.ToLower();
            foreach (var key in responses.Keys)
            {
                if (question.Contains(key))
                {
                    return responses[key];
                }
            }
            return "I'm sorry, I didn't understand that. Can you rephrase?";
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; } = string.Empty;
    }
}
