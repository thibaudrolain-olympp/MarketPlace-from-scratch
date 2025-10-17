const { spawn } = require('child_process');

(async () => {
  try {
    // Prefer puppeteer if installed (provides a compatible Chromium),
    // otherwise fall back to a system-installed chromium binary.
    let chromePath;
    try {
      const puppeteer = require('puppeteer');
      chromePath = puppeteer.executablePath();
      console.log('Using Chromium from puppeteer at:', chromePath);
    } catch (e) {
      // fallback locations
      const candidates = ['/usr/bin/chromium-browser', '/usr/bin/chromium', '/usr/bin/google-chrome'];
      const fs = require('fs');
      chromePath = candidates.find(p => fs.existsSync(p));
      if (chromePath) {
        console.log('Using system Chromium at:', chromePath);
      } else {
        throw new Error('No Chromium binary found. Install puppeteer or system chromium.');
      }
    }

    const env = Object.assign({}, process.env, { CHROME_BIN: chromePath });

    // Run ng test in single-run (non-watch) mode
    const args = ['test', '--watch=false', '--browsers=ChromeHeadless'];
    const path = require('path');
    const fs = require('fs');
    let ngCmd = path.join(process.cwd(), 'node_modules', '.bin', 'ng');
    if (!fs.existsSync(ngCmd)) {
      ngCmd = 'npx';
      // when falling back to npx, use ['ng', ...args]
      args.unshift('ng');
    }

    const proc = spawn(ngCmd, args, { stdio: 'inherit', env });

    proc.on('exit', (code) => {
      process.exit(code);
    });
  } catch (err) {
    console.error('Failed to run tests in CI mode:', err);
    process.exit(1);
  }
})();
