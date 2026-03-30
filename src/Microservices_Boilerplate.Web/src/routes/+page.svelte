<script lang="ts">
    import '../app.css'
</script>

<div class="home-wrapper">
    <section class="hero">
        <div class="hero-left">
            <h1>Welcome to The Roshar Forum</h1>
            <p>
                A place where the Knights Radiant and the people of Roshar gather to discuss and
                share.
            </p>
        </div>
        <div class="hero-right"></div>
    </section>
</div>

<style>
    .home-wrapper {
        margin-top: 4rem;

        .hero {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 2rem;

            h1 {
                font-size: 2.2rem;
                margin-bottom: 0.75rem;
            }

            p {
                color: var(--main-text-color);
                opacity: 0.7;
                font-size: 1rem;
                margin-bottom: 2rem;
                max-width: 540px;
            }
        }

        .hero-right {
            display: flex;
            align-items: center;
            justify-content: center;
        }
    }
</style>
