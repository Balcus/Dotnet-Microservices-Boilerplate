<script lang="ts">
    import '../app.css'
    let { children } = $props()
</script>

<div class="wrapper">
    <nav class="main-nav">
        <a href="/" class="site-title">The Roshar Forum</a>
        <div class="nav-links">
			<a href="/account">Notifications</a>
            <a href="/account">Account</a>
        </div>
    </nav>

    <div class="topics">
        <p>News on:</p>
        <ul>
            <li><a href="/">Shatered Plains</a></li>
            <li><a href="/">Shinovar</a></li>
            <li><a href="/">Alethkar</a></li>
        </ul>
    </div>
</div>

<nav class="feed-nav">
    <ul>
		<li><a href="/">Home</a></li>
        <li><a href="/">Feed</a></li>
        <li><a href="/">Explore</a></li>
        <li><a href="/">News</a></li>
    </ul>
</nav>

<div class="wrapper">
    {@render children()}
</div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Jacquard+12&display=swap');

    .wrapper {
        max-width: 960px;
        margin: 0 auto;
        padding: 2rem 1.5rem;
    }

    .main-nav {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding-bottom: 0.5rem;
    }

    .topics {
        padding: 0.3rem;
        display: flex;
        flex-direction: row;
        align-items: center;
        gap: 1rem;

        ul {
            display: flex;
            flex-direction: row;
            list-style: none;
            gap: 1rem;
        }

        li {
            background-color: #82553ebb;
            padding: 0.3rem;
            border: 1px solid var(--main-text-color);
            border-radius: 8px;
            font-size: 0.8rem;
        }

        li:hover {
            background-color: #82553e;
        }
    }

    .site-title {
        font-family: 'Jacquard 12', system-ui;
        font-size: 3rem;
        text-decoration: none;
    }

    .nav-links {
        display: flex;
        gap: 1.5rem;

        a {
            color: var(--inactive-link-color);
            text-decoration: none;
            font-family: 'MedievalSharp', cursive;
            font-size: 1.2rem;
        }

        a:hover {
            color: var(--active-link-color);
        }
    }

    .feed-nav {
        width: 100%;
        background-color: var(--main-text-color);
        ul {
            display: flex;
            flex-direction: row;
            list-style: none;
            gap: 0;
            padding: 0.5rem 1.5rem;
            max-width: 960px;
            margin: 0 auto;
            li {
                padding: 0 1rem;
                border-left: var(--border-style);
            }
            li:first-child {
                border-left: none;
                padding-left: 0;
            }
            a {
                color: black;
            }
        }
    }
</style>
