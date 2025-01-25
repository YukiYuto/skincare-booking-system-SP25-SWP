import styles from './LoginForm.module.css';
import { InputField } from './InputField';

export function LoginForm() {
  return (
    <form className={styles.loginForm} aria-labelledby="login-title">
      <h1 id="login-title" className={styles.loginTitle}>Login Account</h1>
      
      <InputField
        label="Username"
        type="text"
        id="username"
        placeholder="Username"
      />

      <InputField
        label="Password"
        type="password"
        id="password"
        placeholder="6+ characters"
        showPasswordToggle
      />

      <div className={styles.termsContainer}>
        <span>By signing up you agree to </span>
        <a href="/terms" className={styles.termsLink}>terms and conditions</a>
        <span> at zoho.</span>
      </div>

      <button type="submit" className={styles.loginButton}>
        Login
      </button>

      <button 
        type="button" 
        className={styles.createAccountButton}
        onClick={() => window.location.href = '/signup'}
      >
        Create Account
      </button>
    </form>
  );
}